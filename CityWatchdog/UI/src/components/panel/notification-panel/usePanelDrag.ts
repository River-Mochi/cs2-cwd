// File: src/UI/src/components/panel/notification-panel/usePanelDrag.ts
// Purpose: Keeps the CWD panel draggable, persists its position across game restarts, and clamps
//          it inside the game window so a resolution change can never strand it off-screen.

import { useCallback, useEffect, useRef, useState, type MouseEvent as ReactMouseEvent } from "react";
import { OnPanelPositionChanged } from "../../../bindings/bindings";

type PanelOffset = {
    x: number;
    y: number;
};

type DragState = {
    pointerX: number;
    pointerY: number;
    originX: number;
    originY: number;
    originLeft: number;
    originTop: number;
    originWidth: number;
    originHeight: number;
};

// Module scope so the panel keeps its place when closed and reopened within one session.
let sessionPanelOffset: PanelOffset = { x: 0, y: 0 };

export const usePanelDrag = (
    savedOffset: PanelOffset,
    onPositionChanged: (x: number, y: number) => void = OnPanelPositionChanged,
) => {
    const [panelOffset, setPanelOffset] = useState<PanelOffset>(sessionPanelOffset);
    const [panelDragging, setPanelDragging] = useState(false);
    const panelElementRef = useRef<HTMLDivElement | null>(null);
    const dragStateRef = useRef<DragState | null>(null);
    const pendingOffsetRef = useRef(panelOffset);
    const animationFrameRef = useRef<number | null>(null);
    const clampFrameRef = useRef<number | null>(null);
    const clampTimeoutRef = useRef<number | null>(null);

    const applyPanelOffset = useCallback((next: PanelOffset) => {
        sessionPanelOffset = next;
        pendingOffsetRef.current = next;
        setPanelOffset(next);
    }, []);

    // Nudge the panel back inside the viewport if any edge is off-screen. Session-only (never saved),
    // so the player's saved position is preserved for when they return to a larger resolution.
    const clampPanelOffset = useCallback(() => {
        const rect = panelElementRef.current?.getBoundingClientRect();
        if (rect === undefined) {
            return;
        }

        const current = pendingOffsetRef.current;
        let nextX = current.x;
        let nextY = current.y;

        if (rect.left < 0) {
            nextX -= rect.left;
        }
        if (rect.top < 0) {
            nextY -= rect.top;
        }
        if (rect.right > window.innerWidth) {
            nextX -= rect.right - window.innerWidth;
        }
        if (rect.bottom > window.innerHeight) {
            nextY -= rect.bottom - window.innerHeight;
        }

        if (nextX === current.x && nextY === current.y) {
            return;
        }

        applyPanelOffset({ x: nextX, y: nextY });
    }, [applyPanelOffset]);

    const scheduleClamp = useCallback(() => {
        if (clampFrameRef.current !== null) {
            window.cancelAnimationFrame(clampFrameRef.current);
        }
        clampFrameRef.current = window.requestAnimationFrame(() => {
            clampFrameRef.current = null;
            clampPanelOffset();
        });
    }, [clampPanelOffset]);

    // Restore the saved position once the C# binding resolves, then clamp it on-screen after layout.
    // A short follow-up timeout catches Cohtml layouts that settle a frame or two late.
    useEffect(() => {
        applyPanelOffset({ x: savedOffset.x, y: savedOffset.y });
        scheduleClamp();

        if (clampTimeoutRef.current !== null) {
            window.clearTimeout(clampTimeoutRef.current);
        }
        clampTimeoutRef.current = window.setTimeout(() => {
            clampTimeoutRef.current = null;
            clampPanelOffset();
        }, 150);
    }, [applyPanelOffset, clampPanelOffset, scheduleClamp, savedOffset.x, savedOffset.y]);

    // Re-clamp when the window is resized (resolution or windowed/fullscreen change).
    useEffect(() => {
        window.addEventListener("resize", scheduleClamp);
        return () => {
            window.removeEventListener("resize", scheduleClamp);
        };
    }, [scheduleClamp]);

    useEffect(() => {
        if (!panelDragging) {
            return;
        }

        const onMouseMove = (event: MouseEvent) => {
            const dragState = dragStateRef.current;
            if (dragState === null) {
                return;
            }

            const deltaX = event.clientX - dragState.pointerX;
            const deltaY = event.clientY - dragState.pointerY;
            let nextX = dragState.originX + deltaX;
            let nextY = dragState.originY + deltaY;
            const nextLeft = dragState.originLeft + deltaX;
            const nextTop = dragState.originTop + deltaY;
            const nextRight = nextLeft + dragState.originWidth;
            const nextBottom = nextTop + dragState.originHeight;

            if (nextLeft < 0) {
                nextX -= nextLeft;
            }
            if (nextTop < 0) {
                nextY -= nextTop;
            }
            if (nextRight > window.innerWidth) {
                nextX -= nextRight - window.innerWidth;
            }
            if (nextBottom > window.innerHeight) {
                nextY -= nextBottom - window.innerHeight;
            }

            pendingOffsetRef.current = { x: nextX, y: nextY };
            if (animationFrameRef.current === null) {
                animationFrameRef.current = window.requestAnimationFrame(() => {
                    animationFrameRef.current = null;
                    applyPanelOffset(pendingOffsetRef.current);
                });
            }
        };

        const onMouseUp = () => {
            if (animationFrameRef.current !== null) {
                window.cancelAnimationFrame(animationFrameRef.current);
                animationFrameRef.current = null;
            }

            dragStateRef.current = null;
            setPanelDragging(false);
            applyPanelOffset(pendingOffsetRef.current);
            // Persist the final position so the panel returns here after a game restart.
            onPositionChanged(pendingOffsetRef.current.x, pendingOffsetRef.current.y);
        };

        window.addEventListener("mousemove", onMouseMove);
        window.addEventListener("mouseup", onMouseUp);

        return () => {
            window.removeEventListener("mousemove", onMouseMove);
            window.removeEventListener("mouseup", onMouseUp);
        };
    }, [applyPanelOffset, onPositionChanged, panelDragging]);

    useEffect(() => () => {
        if (animationFrameRef.current !== null) {
            window.cancelAnimationFrame(animationFrameRef.current);
        }
        if (clampFrameRef.current !== null) {
            window.cancelAnimationFrame(clampFrameRef.current);
        }
        if (clampTimeoutRef.current !== null) {
            window.clearTimeout(clampTimeoutRef.current);
        }
    }, []);

    const handlePanelDragStart = useCallback((event: ReactMouseEvent<HTMLDivElement>) => {
        event.preventDefault();
        event.stopPropagation();

        const rect = panelElementRef.current?.getBoundingClientRect();
        if (rect === undefined) {
            return;
        }

        pendingOffsetRef.current = panelOffset;
        dragStateRef.current = {
            pointerX: event.clientX,
            pointerY: event.clientY,
            originX: panelOffset.x,
            originY: panelOffset.y,
            originLeft: rect.left,
            originTop: rect.top,
            originWidth: rect.width,
            originHeight: rect.height,
        };
        setPanelDragging(true);
    }, [panelOffset]);

    return {
        panelOffset,
        panelDragging,
        panelElementRef,
        handlePanelDragStart,
    };
};
