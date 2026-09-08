// File: src/UI/src/components/panel/notification-panel/notificationPanelFrame.tsx
// Purpose: CWD panel frame, title bar, drag, and shared tooltip wrapper.

import { useValue } from "cs2/api";
import { getModule } from "cs2/modding";
import { Button, FormattedParagraphs, Panel, Tooltip } from "cs2/ui";
import type { ReactElement, ReactNode } from "react";
import {
  disableCwdTooltips$,
  interfaceScaleEnabled$,
  mainPanelOpacity$,
  OnDisableCwdTooltipsToggle,
  OnToggleInterfaceScale,
} from "../../../bindings/bindings";
import { VanillaComponentResolver } from "../../../utils/vanilla";
import { playSelectSound } from "../../../utils/uiSound";
import ScalePanelsPath from "../../../../images/ScalePanels.svg";
import TitleBarIconPath from "../../../../images/NotificationIcon_TitleBar.svg";
import styles from "./notificationPanel.module.scss";
import type { Localize } from "./notificationData";
import { usePanelDrag } from "./usePanelDrag";

const roundButtonHighlightStyle = getModule(
  "game-ui/common/input/button/themes/round-highlight-button.module.scss",
  "classes",
);

const modIconSrc = TitleBarIconPath;
const scalePanelsSrc = ScalePanelsPath;

const getMainPanelOpacityClass = (value: number) => {
  const normalized = Math.round(
    Math.min(100, Math.max(30, Number.isFinite(value) ? value : 80)) / 5,
  ) * 5;

  return styles[`opacity${normalized}`] ?? styles.opacity80;
};

const renderTooltipLines = (tooltip: ReactNode): ReactNode =>
  typeof tooltip === "string" && tooltip.includes("\n")
    ? <FormattedParagraphs className={styles.tooltipParagraphs}>{tooltip}</FormattedParagraphs>
    : tooltip;

// CWD tooltips stay separate from the game's global tooltip toggle.
export const CwdTooltip = ({
  tooltip,
  alwaysVisible,
  children,
}: {
  tooltip: ReactNode;
  alwaysVisible?: boolean;
  children: ReactElement;
}) => {
  const cwdTooltipsDisabled = useValue(disableCwdTooltips$);

  if (cwdTooltipsDisabled && !alwaysVisible) {
    return <>{children}</>;
  }

  return (
    <Tooltip {...{ cwdBypass: true }} tooltip={renderTooltipLines(tooltip)}>
      {children}
    </Tooltip>
  );
};

export const DraggablePanelFrame = ({
  savedOffset,
  localize,
  panelTitle,
  panelCollapsed,
  allSectionsExpanded,
  onPanelCollapsedToggle,
  onCloseClick,
  children,
}: {
  savedOffset: { x: number; y: number };
  localize: Localize;
  panelTitle: string;
  panelCollapsed: boolean;
  allSectionsExpanded: boolean;
  onPanelCollapsedToggle: () => void;
  onCloseClick: () => void;
  children: ReactNode;
}) => {
  const mainPanelOpacity = useValue(mainPanelOpacity$);
  const cwdTooltipsDisabled = useValue(disableCwdTooltips$);
  const interfaceScaleEnabled = useValue(interfaceScaleEnabled$);
  const mainPanelOpacityClass = getMainPanelOpacityClass(mainPanelOpacity);

  const {
    panelOffset,
    panelDragging,
    panelElementRef,
    handlePanelDragStart,
  } = usePanelDrag(savedOffset);

  const titleBarTooltip = cwdTooltipsDisabled
    ? localize(
      "TitleBarTooltipPanelOff",
      "City Watchdog tooltips are OFF.\nClick to turn them back on.",
    )
    : localize(
      "TitleBarTooltipPanelOn",
      "City Watchdog tooltips are ON.\nTurn them off in Options → City Watchdog → Main.",
    );

  const scaleTooltip = interfaceScaleEnabled
    ? localize(
      "InterfaceScaleOn",
      "Bigger UI is ON.\nClick to return the game interface to normal size.",
    )
    : localize(
      "InterfaceScaleOff",
      "Make the whole game interface bigger — panels and text.\nAffects the entire game and stays on until you turn it off.",
    );

  const panelCollapseTooltip = localize("PanelCollapseToggle", "Expand/collapse whole panel.");
  const dragTitleTooltip = localize("DragTitleBar", "Drag title bar.");

  return (
    <div
      ref={panelElementRef}
      className={styles.panelAnchor}
      style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
    >
      <Panel
        className={`${styles.panel} ${mainPanelOpacityClass} ${allSectionsExpanded ? styles.panelAllExpanded : ""}`}
        header={
          <div className={styles.header}>
            <div className={styles.headerTitleArea}>
              {/* Paw restores CWD tooltips. Options turns them off. */}
              <CwdTooltip tooltip={titleBarTooltip} alwaysVisible>
                <div
                  className={`${styles.headerModIconButton} ${cwdTooltipsDisabled ? styles.headerModIconOff : ""}`}
                  role="button"
                  aria-pressed={cwdTooltipsDisabled}
                  onClick={() => {
                    if (!cwdTooltipsDisabled) {
                      return;
                    }

                    playSelectSound();
                    OnDisableCwdTooltipsToggle(false);
                  }}
                >
                  <img src={modIconSrc} className={styles.headerModIcon} />
                </div>
              </CwdTooltip>

              {/* Vanilla UI scale, exposed without dev mode. */}
              <CwdTooltip tooltip={scaleTooltip} alwaysVisible>
                <div
                  className={`${styles.headerScaleButton} ${interfaceScaleEnabled ? styles.headerScaleButtonActive : ""}`}
                  role="button"
                  aria-pressed={interfaceScaleEnabled}
                  onClick={() => {
                    playSelectSound();
                    OnToggleInterfaceScale(!interfaceScaleEnabled);
                  }}
                >
                  <img src={scalePanelsSrc} className={styles.headerScaleIcon} />
                </div>
              </CwdTooltip>

              {/* No tooltip wrapper while dragging; it flickers behind fast mouse moves. */}
              {panelDragging ? (
                <div
                  className={`${styles.headerModName} ${styles.headerModNameDragging}`}
                  onMouseDown={handlePanelDragStart}
                >
                  {panelTitle}
                </div>
              ) : (
                <CwdTooltip tooltip={dragTitleTooltip}>
                  <div
                    className={styles.headerModName}
                    onMouseDown={handlePanelDragStart}
                  >
                    {panelTitle}
                  </div>
                </CwdTooltip>
              )}
            </div>

            <CwdTooltip tooltip={panelCollapseTooltip}>
              <Button
                className={roundButtonHighlightStyle.button + " " + styles.headerCollapseButton}
                variant="icon"
                onSelect={onPanelCollapsedToggle}
                focusKey={VanillaComponentResolver.instance.FOCUS_DISABLED}
              >
                <img
                  src={
                    panelCollapsed
                      ? "Media/Glyphs/ThickStrokeArrowRight.svg"
                      : "Media/Glyphs/ThickStrokeArrowDown.svg"
                  }
                  className={styles.headerCollapseIcon}
                />
              </Button>
            </CwdTooltip>

            <Button
              className={roundButtonHighlightStyle.button + " " + styles.headerCloseButton}
              variant="icon"
              onSelect={onCloseClick}
              focusKey={VanillaComponentResolver.instance.FOCUS_DISABLED}
            >
              <img src="Media/Glyphs/Close.svg" className={styles.headerCloseIcon} />
            </Button>
          </div>
        }
      >
        {children}
      </Panel>
    </div>
  );
};
