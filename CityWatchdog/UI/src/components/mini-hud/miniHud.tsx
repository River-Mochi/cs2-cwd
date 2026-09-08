// File: src/UI/src/components/mini-hud/miniHud.tsx
// Purpose: Compact configurable notification totals HUD.

import { useValue } from "cs2/api";
import { game } from "cs2/bindings";
import { Tooltip } from "cs2/ui";
import { playSelectSound } from "../../utils/uiSound";
import { useCallback, useEffect, useRef, useState, type MouseEvent as ReactMouseEvent } from "react";
import {
    miniHudEnabled$,
    miniHudFavorites$,
    miniHudHideZero$,
    miniHudItemCount$,
    miniHudMode$,
    miniHudOrientation$,
    miniHudPanelOpacity$,
    miniHudPanelStyle$,
    miniHudPlacement$,
    miniHudHorizontalPositionX$,
    miniHudHorizontalPositionY$,
    miniHudScale$,
    miniHudVerticalPositionX$,
    miniHudVerticalPositionY$,
    notificationCounts$,
    OnMiniHudNotificationClicked,
    OnMiniHudPositionChanged,
} from "../../bindings/bindings";
import { allItems } from "../panel/notification-panel/notificationData";
import { useText } from "../shared/localization";
import { formatMiniNotificationCount } from "../shared/formatNotificationCount";
import styles from "./miniHud.module.scss";
import EmptyIconPath from "../../../images/NotificationIcon_TitleBar.svg";

const MODE_FAVORITES = 1;
const ORIENTATION_VERTICAL = 1;
const PLACEMENT_TOP_CENTER = 0;
const PLACEMENT_TOP_RIGHT = 1;
const PLACEMENT_DRAGGABLE = 2;
const PANEL_STYLE_GLASS = 1;
const MINI_HUD_POSITION_LIMIT = 20000;

type Position = {
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

let sessionPosition: Position = { x: 0, y: 0 };

const getMiniHudScaleClass = (value: number) => {
    const normalized = Math.round(Math.min(130, Math.max(90, Number.isFinite(value) ? value : 100)) / 5) * 5;
    return styles[`scale${normalized}`] ?? styles.scale100;
};

const getMiniHudOpacityClass = (value: number) => {
    const normalized = Math.round(Math.min(100, Math.max(30, Number.isFinite(value) ? value : 70)) / 5) * 5;
    return styles[`opacity${normalized}`] ?? styles.opacity70;
};

const normalizePositionValue = (value: number) =>
    Math.round(Math.min(MINI_HUD_POSITION_LIMIT, Math.max(-MINI_HUD_POSITION_LIMIT, Number.isFinite(value) ? value : 0)));

export const MiniHud = () => {
    const text = useText();
    const enabled = useValue(miniHudEnabled$);
    const counts = useValue(notificationCounts$);
    const favorites = useValue(miniHudFavorites$);
    const mode = useValue(miniHudMode$);
    const itemCount = useValue(miniHudItemCount$);
    const orientation = useValue(miniHudOrientation$);
    const placement = useValue(miniHudPlacement$);
    const miniHudScale = useValue(miniHudScale$);
    const hideZero = useValue(miniHudHideZero$);
    const panelStyle = useValue(miniHudPanelStyle$);
    const panelOpacity = useValue(miniHudPanelOpacity$);
    const savedHorizontalPositionX = useValue(miniHudHorizontalPositionX$);
    const savedHorizontalPositionY = useValue(miniHudHorizontalPositionY$);
    const savedVerticalPositionX = useValue(miniHudVerticalPositionX$);
    const savedVerticalPositionY = useValue(miniHudVerticalPositionY$);
    const activeGamePanel = useValue(game.activeGamePanel$);
    const activeGamePanelType = activeGamePanel?.__Type ?? "none";
    const isPhotoMode = activeGamePanelType == game.GamePanelType.PhotoMode;
    const isDraggable = placement === PLACEMENT_DRAGGABLE;
    const [position, setPosition] = useState<Position>(sessionPosition);
    const [dragging, setDragging] = useState(false);
    const hudRef = useRef<HTMLDivElement | null>(null);
    const dragRef = useRef<DragState | null>(null);
    const pendingPositionRef = useRef(position);
    const animationFrameRef = useRef<number | null>(null);
    const resizeAnimationFrameRef = useRef<number | null>(null);
    const layoutAnimationFrameRef = useRef<number | null>(null);
    const layoutTimeoutRef = useRef<number | null>(null);

    const applyHudPosition = useCallback((next: Position) => {
        sessionPosition = next;
        pendingPositionRef.current = next;
        setPosition(next);
    }, []);

    const resetHudPosition = useCallback(() => {
        applyHudPosition({ x: 0, y: 0 });
    }, [applyHudPosition]);

    const restoreSavedHudPosition = useCallback(() => {
        const savedX = orientation === ORIENTATION_VERTICAL ? savedVerticalPositionX : savedHorizontalPositionX;
        const savedY = orientation === ORIENTATION_VERTICAL ? savedVerticalPositionY : savedHorizontalPositionY;

        applyHudPosition({
            x: normalizePositionValue(savedX),
            y: normalizePositionValue(savedY),
        });
    }, [applyHudPosition, orientation, savedHorizontalPositionX, savedHorizontalPositionY, savedVerticalPositionX, savedVerticalPositionY]);

    const clampHudPosition = useCallback(() => {
        const rect = hudRef.current?.getBoundingClientRect();
        if (rect === undefined) {
            return;
        }

        const current = pendingPositionRef.current;
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

        const next = { x: nextX, y: nextY };
        applyHudPosition(next);
    }, [applyHudPosition]);

    useEffect(() => {
        if (isDraggable) {
            restoreSavedHudPosition();
            return;
        }

        resetHudPosition();
    }, [isDraggable, orientation, placement, resetHudPosition, restoreSavedHudPosition]);

    useEffect(() => {
        if (!dragging) {
            return;
        }

        const onMouseMove = (event: MouseEvent) => {
            const drag = dragRef.current;
            if (drag === null) {
                return;
            }

            const deltaX = event.clientX - drag.pointerX;
            const deltaY = event.clientY - drag.pointerY;

            let nextX = drag.originX + deltaX;
            let nextY = drag.originY + deltaY;
            const nextLeft = drag.originLeft + deltaX;
            const nextTop = drag.originTop + deltaY;
            const nextRight = nextLeft + drag.originWidth;
            const nextBottom = nextTop + drag.originHeight;

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

            pendingPositionRef.current = { x: nextX, y: nextY };
            if (animationFrameRef.current === null) {
                animationFrameRef.current = window.requestAnimationFrame(() => {
                    animationFrameRef.current = null;
                    applyHudPosition(pendingPositionRef.current);
                });
            }
        };

        const onMouseUp = () => {
            if (animationFrameRef.current !== null) {
                window.cancelAnimationFrame(animationFrameRef.current);
                animationFrameRef.current = null;
            }

            dragRef.current = null;
            setDragging(false);
            applyHudPosition(pendingPositionRef.current);
            if (isDraggable) {
                OnMiniHudPositionChanged(orientation, pendingPositionRef.current.x, pendingPositionRef.current.y);
            }
        };

        document.addEventListener("mousemove", onMouseMove);
        document.addEventListener("mouseup", onMouseUp);
        return () => {
            document.removeEventListener("mousemove", onMouseMove);
            document.removeEventListener("mouseup", onMouseUp);
        };
    }, [applyHudPosition, dragging, isDraggable, orientation]);

    useEffect(() => {
        if (!enabled || !isDraggable) {
            return;
        }

        const onResize = () => {
            if (resizeAnimationFrameRef.current !== null) {
                return;
            }

            resizeAnimationFrameRef.current = window.requestAnimationFrame(() => {
                resizeAnimationFrameRef.current = null;
                resetHudPosition();
            });
        };

        window.addEventListener("resize", onResize);
        return () => {
            window.removeEventListener("resize", onResize);
            if (resizeAnimationFrameRef.current !== null) {
                window.cancelAnimationFrame(resizeAnimationFrameRef.current);
                resizeAnimationFrameRef.current = null;
            }
        };
    }, [enabled, isDraggable, resetHudPosition]);

    useEffect(() => () => {
        if (animationFrameRef.current !== null) {
            window.cancelAnimationFrame(animationFrameRef.current);
        }
        if (resizeAnimationFrameRef.current !== null) {
            window.cancelAnimationFrame(resizeAnimationFrameRef.current);
        }
        if (layoutAnimationFrameRef.current !== null) {
            window.cancelAnimationFrame(layoutAnimationFrameRef.current);
        }
        if (layoutTimeoutRef.current !== null) {
            window.clearTimeout(layoutTimeoutRef.current);
        }
    }, []);

    const favoriteSet = new Set(favorites);
    const maxItems = itemCount === 10 ? 10 : 5;
    // index here is item.countIndex (the stable C#-side index), not this item's position in `allItems` —
    // sections can be reorganized for display (e.g. Leveling Building lives in "Other") without this
    // breaking, since countIndex travels with the item regardless of which section it's declared in.
    const candidatePool = allItems
        .map((item) => ({
            item,
            index: item.countIndex,
            count: counts[item.countIndex] ?? 0,
        }))
        .filter((entry) => mode !== MODE_FAVORITES || favoriteSet.has(entry.index))
        .filter((entry) => !hideZero || entry.count > 0)
        .sort((a, b) => b.count - a.count || a.index - b.index);

    const seenMiniHudIdentities = new Set<string>();
    const candidates = candidatePool
        .filter((entry) => {
            const identity = entry.item.miniHudIdentity ?? entry.item.localeId;
            if (seenMiniHudIdentities.has(identity)) {
                return false;
            }

            seenMiniHudIdentities.add(identity);
            return true;
        })
        .slice(0, maxItems);

    const candidateLayoutKey = candidates.length === 0
        ? "empty"
        : candidates.map(({ index, count }) => `${index}:${formatMiniNotificationCount(count)}`).join("|");
    const layoutOptionsKey = `${activeGamePanelType}:${mode}:${maxItems}:${hideZero ? 1 : 0}:${favorites.join(",")}`;

    useEffect(() => {
        if (!enabled || !isDraggable || dragging) {
            return;
        }

        const scheduleClamp = () => {
            if (layoutAnimationFrameRef.current !== null) {
                window.cancelAnimationFrame(layoutAnimationFrameRef.current);
            }

            layoutAnimationFrameRef.current = window.requestAnimationFrame(() => {
                layoutAnimationFrameRef.current = null;
                clampHudPosition();
            });
        };

        if (layoutTimeoutRef.current !== null) {
            window.clearTimeout(layoutTimeoutRef.current);
            layoutTimeoutRef.current = null;
        }

        scheduleClamp();
        layoutTimeoutRef.current = window.setTimeout(() => {
            layoutTimeoutRef.current = null;
            scheduleClamp();
        }, 150);

        return () => {
            if (layoutAnimationFrameRef.current !== null) {
                window.cancelAnimationFrame(layoutAnimationFrameRef.current);
                layoutAnimationFrameRef.current = null;
            }
            if (layoutTimeoutRef.current !== null) {
                window.clearTimeout(layoutTimeoutRef.current);
                layoutTimeoutRef.current = null;
            }
        };
    }, [candidateLayoutKey, clampHudPosition, dragging, enabled, isDraggable, layoutOptionsKey, miniHudScale, orientation, panelOpacity, panelStyle]);

    if (!enabled || isPhotoMode) {
        return null;
    }

    const onOpenHandleMouseDown = (event: ReactMouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        event.stopPropagation();

        if (!isDraggable) {
            return;
        }

        const rect = hudRef.current?.getBoundingClientRect();
        if (rect === undefined) {
            return;
        }

        pendingPositionRef.current = position;
        dragRef.current = {
            pointerX: event.clientX,
            pointerY: event.clientY,
            originX: position.x,
            originY: position.y,
            originLeft: rect.left,
            originTop: rect.top,
            originWidth: rect.width,
            originHeight: rect.height,
        };
        setDragging(true);
    };

    const onOpenHandleClick = (event: ReactMouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        event.stopPropagation();
    };

    const onItemClick = (event: ReactMouseEvent<HTMLDivElement>, index: number) => {
        event.preventDefault();
        event.stopPropagation();
        playSelectSound();
        OnMiniHudNotificationClicked(index);
    };

    const placementClass =
        placement === PLACEMENT_TOP_CENTER
            ? styles.topCenter
            : placement === PLACEMENT_TOP_RIGHT
                ? styles.topRight
                : orientation === ORIENTATION_VERTICAL
                    ? styles.draggableVertical
                    : styles.draggableHorizontal;
    const dragTransform = orientation === ORIENTATION_VERTICAL
        ? `translate(${position.x}px, ${position.y}px)`
        : `translate(-50%, 0) translate(${position.x}px, ${position.y}px)`;
    const scaleClass = getMiniHudScaleClass(miniHudScale);
    const panelStyleClass = panelStyle === PANEL_STYLE_GLASS ? styles.glass : styles.dark;
    const panelOpacityClass = getMiniHudOpacityClass(panelOpacity);
    const openHandleTooltip = text("MiniHudDragHandle", "Draggable dots.");
    const openHandleButton = (
        <button
            type="button"
            className={styles.openHandle}
            onMouseDown={onOpenHandleMouseDown}
            onClick={onOpenHandleClick}
            aria-label={openHandleTooltip}
        >
            <span className={styles.openHandleMark} aria-hidden="true">
                <span className={styles.openHandleDot}></span>
                <span className={styles.openHandleDot}></span>
                <span className={styles.openHandleDot}></span>
            </span>
        </button>
    );

    return (
        <div
            ref={hudRef}
            className={`${styles.hud} ${panelStyleClass} ${panelOpacityClass} ${orientation === ORIENTATION_VERTICAL ? styles.vertical : styles.horizontal} ${scaleClass} ${isDraggable ? styles.draggable : ""} ${placementClass}`}
            style={isDraggable
                ? { transform: dragTransform }
                : undefined}
        >
            <div className={styles.content}>
                {/* MINI HUD counts — separate from the main panel's notificationRow count. */}
                <div className={styles.items}>
                    {candidates.length === 0 ? (
                        <div
                            className={styles.item}
                            role="button"
                        >
                            <span className={styles.count}>0</span>
                            <img src={EmptyIconPath} className={styles.icon} alt="" />
                        </div>
                    ) : candidates.map(({ item, index, count }) => (
                        <div
                            key={index}
                            className={styles.item}
                            role="button"
                            onClick={(event) => onItemClick(event, index)}
                        >
                            <span className={styles.count}>{formatMiniNotificationCount(count)}</span>
                            <img src={item.icon} className={styles.icon} alt="" />
                        </div>
                    ))}
                </div>
                {isDraggable && (
                  <Tooltip {...{ cwdBypass: true }} tooltip={openHandleTooltip}>
                    {openHandleButton}
                  </Tooltip>
                )}
            </div>
        </div>
    );
};
