// File: src/UI/src/components/editor-quick-controls/editorQuickControls.tsx
// Purpose: Compact Editor-only CWD quick controls, opened with the normal panel hotkey.

import { useValue } from "cs2/api";
import { Tooltip } from "cs2/ui";
import { type ReactElement, type ReactNode } from "react";
import {
  disableAllTooltips$,
  disableCwdTooltips$,
  editorQuickControlsEnabled$,
  editorQuickControlsPositionX$,
  editorQuickControlsPositionY$,
  hideRoadNames$,
  interfaceScaleEnabled$,
  mainPanelOpacity$,
  OnDisableAllTooltipsToggle,
  OnEditorQuickControlsPositionChanged,
  OnHideRoadNamesToggle,
  OnShowRoadArrowsToggle,
  OnToggleInterfaceScale,
  showRoadArrows$,
} from "../../bindings/bindings";
import { PanelButton } from "../panel/notification-panel/buttons/panelButton";
import { usePanelDrag } from "../panel/notification-panel/usePanelDrag";
import styles from "./editorQuickControls.module.scss";

import RoadNameOnPath from "../../../images/icon-RoadName-max.svg";
import RoadArrowIconPath from "../../../images/icon-RoadArrows-max.svg";
import ScalePanelsPath from "../../../images/ScalePanels.svg";

const infoIconSrc = "Media/Game/Icons/AdvisorInfoViewWhite.svg";

const getPanelOpacityClass = (value: number) => {
  const normalized = Math.round(
    Math.min(100, Math.max(30, Number.isFinite(value) ? value : 80)) / 5,
  ) * 5;

  return styles[`opacity${normalized}`] ?? styles.opacity80;
};

const QuickTooltip = ({
  tooltip,
  disabled,
  children,
}: {
  tooltip: ReactNode;
  disabled: boolean;
  children: ReactElement;
}) => disabled
  ? <>{children}</>
  : <Tooltip {...{ cwdBypass: true }} tooltip={tooltip}>{children}</Tooltip>;

export const EditorQuickControls = () => {
  const visible = useValue(editorQuickControlsEnabled$);
  const savedPositionX = useValue(editorQuickControlsPositionX$);
  const savedPositionY = useValue(editorQuickControlsPositionY$);
  const cwdTooltipsDisabled = useValue(disableCwdTooltips$);
  const allTooltipsDisabled = useValue(disableAllTooltips$);
  const interfaceScaleEnabled = useValue(interfaceScaleEnabled$);
  const mainPanelOpacity = useValue(mainPanelOpacity$);
  const roadNamesHidden = useValue(hideRoadNames$);
  const roadArrowsShown = useValue(showRoadArrows$);
  const {
    panelOffset,
    panelDragging,
    panelElementRef,
    handlePanelDragStart,
  } = usePanelDrag(
    { x: savedPositionX, y: savedPositionY },
    OnEditorQuickControlsPositionChanged,
  );

  if (!visible) {
    return null;
  }

  const tooltipsDisabled = cwdTooltipsDisabled || panelDragging;

  return (
    <div
      ref={panelElementRef}
      className={styles.anchor}
      style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
      data-asl-marker-tooltip-block="true"
    >
      <div className={`${styles.panel} ${getPanelOpacityClass(mainPanelOpacity)}`}>
        <QuickTooltip
          disabled={panelDragging}
          tooltip="Drag the title bar."
        >
          <div
            className={`${styles.title} ${panelDragging ? styles.titleDragging : ""}`}
            onMouseDown={handlePanelDragStart}
          >
            City Watchdog Editor
          </div>
        </QuickTooltip>

        <div
          className={styles.controls}
          onMouseDown={(event) => event.stopPropagation()}
        >
          <QuickTooltip
            disabled={panelDragging}
            tooltip={allTooltipsDisabled ? "Show all game hover tooltips." : "Hide all game hover tooltips."}
          >
            <PanelButton
              tone={allTooltipsDisabled ? "danger" : "default"}
              ariaPressed={allTooltipsDisabled}
              iconSrc={infoIconSrc}
              iconAlt="Show or hide game tooltips"
              onClick={() => OnDisableAllTooltipsToggle(!allTooltipsDisabled)}
            />
          </QuickTooltip>

          <QuickTooltip
            disabled={tooltipsDisabled}
            tooltip={interfaceScaleEnabled
              ? "Bigger Game UI is ON — click to return to normal size."
              : "Make the whole game UI bigger — click to turn it on."}
          >
            <PanelButton
              tone={interfaceScaleEnabled ? "active" : "default"}
              ariaPressed={interfaceScaleEnabled}
              iconSrc={ScalePanelsPath}
              iconAlt="Bigger Game UI"
              iconClassName={styles.darkIcon}
              onClick={() => OnToggleInterfaceScale(!interfaceScaleEnabled)}
            />
          </QuickTooltip>

          <QuickTooltip
            disabled={tooltipsDisabled}
            tooltip={roadNamesHidden ? "Show road names." : "Hide road names."}
          >
            <PanelButton
              tone={roadNamesHidden ? "active" : "default"}
              ariaPressed={roadNamesHidden}
              iconSrc={RoadNameOnPath}
              iconAlt="Hide or show road names"
              iconKind="map"
              onClick={() => OnHideRoadNamesToggle(!roadNamesHidden)}
            />
          </QuickTooltip>

          <QuickTooltip
            disabled={tooltipsDisabled}
            tooltip={roadArrowsShown ? "Hide always-on 1-way road arrows." : "Show 1-way road arrows."}
          >
            <PanelButton
              tone={roadArrowsShown ? "active" : "default"}
              ariaPressed={roadArrowsShown}
              iconSrc={RoadArrowIconPath}
              iconAlt="Show or hide one-way road arrows"
              iconKind="map"
              onClick={() => OnShowRoadArrowsToggle(!roadArrowsShown)}
            />
          </QuickTooltip>
        </div>
      </div>
    </div>
  );
};
