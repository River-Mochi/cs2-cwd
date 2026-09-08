// File: src/UI/src/components/panel/notification-panel/notificationPanelToolbar.tsx
// Purpose: CWD main-panel toolbar buttons.

import { useValue } from "cs2/api";
import {
  activePreset$,
  disableAllTooltips$,
  hideDistrictNames$,
  hideRoadNames$,
  preset1Saved$,
  preset2Saved$,
  showRoadArrows$,
  OnDisableAllTooltipsToggle,
  OnHideDistrictNamesToggle,
  OnHideRoadNamesToggle,
  OnLoadPreset,
  OnSavePreset,
  OnShowRoadArrowsToggle,
} from "../../../bindings/bindings";
import DistrictIconPath from "../../../../images/Districts-max.svg";
import RoadArrowIconPath from "../../../../images/icon-RoadArrows-max.svg";
import RoadNameOnPath from "../../../../images/icon-RoadName-max.svg";
import SortActivePath from "../../../../images/sort-active.svg";
import SortArrowDownPath from "../../../../images/sort-arrow-down.svg";
import SortArrowUpPath from "../../../../images/sort-arrow-up.svg";
import { PanelButton, PanelButtonText, type PanelButtonTone } from "./buttons/panelButton";
import { PresetSlot } from "./buttons/presetButtons";
import presetStyles from "./buttons/presetButtons.module.scss";
import styles from "./notificationPanel.module.scss";
import { CwdTooltip } from "./notificationPanelFrame";
import { allIconSources, setAllNotifications, type Localize } from "./notificationData";

const infoIconSrc = "Media/Game/Icons/AdvisorInfoViewWhite.svg";
const roadNameOnSrc = RoadNameOnPath;
const districtIconSrc = DistrictIconPath;
const roadArrowIconSrc = RoadArrowIconPath;

const preloadedIconSources = [
  ...allIconSources,
  SortArrowUpPath,
  SortArrowDownPath,
  SortActivePath,
];

export const NotificationPanelToolbar = ({
  localize,
  sortIconSrc,
  sortTooltip,
  activeSort,
  allSectionsExpanded,
  selectedTotalCount,
  totalNotificationCount,
  toggleAllTone,
  allSelected,
  onSortButtonClick,
  onExitToGroupedView,
  onToggleAllSections,
}: {
  localize: Localize;
  sortIconSrc: string;
  sortTooltip: string;
  activeSort: boolean;
  allSectionsExpanded: boolean;
  selectedTotalCount: number;
  totalNotificationCount: number;
  toggleAllTone: PanelButtonTone;
  allSelected: boolean;
  onSortButtonClick: () => void;
  onExitToGroupedView: () => void;
  onToggleAllSections: () => void;
}) => {
  const allTooltipsDisabled = useValue(disableAllTooltips$);
  const roadNamesHidden = useValue(hideRoadNames$);
  const districtNamesHidden = useValue(hideDistrictNames$);
  const roadArrowsShown = useValue(showRoadArrows$);
  const preset1Saved = useValue(preset1Saved$);
  const preset2Saved = useValue(preset2Saved$);
  const activePreset = useValue(activePreset$);

  const infoTooltip = localize(
    "TooltipToggle",
    "Show/hide ALL Game hover tooltips.\nDoes not include City Watchdog tooltips.",
  );

  const roadNameTooltip = roadNamesHidden
    ? localize("RoadNameToggleOff", "Click to show road names.\nHotkey: \\")
    : localize("RoadNameToggleOn", "Click to hide road names.\nHotkey: \\");

  const roadArrowTooltip = localize(
    "RoadArrowToggleOff",
    "Click to show/hide 1-way road arrows on every road.\nThis also hides road names as side effect.\nNormally only visible while a road tool is active.",
  );

  const districtNameTooltip = districtNamesHidden
    ? localize("DistrictNameToggleOff", "Click to show district names.")
    : localize("DistrictNameToggleOn", "Click to hide district names.");

  const savedPresetTooltip = localize(
    "PresetLoadHint",
    "Click to load this saved icon setup.\nHold 1 second to overwrite it with your current checkboxes.",
  );
  const emptyPresetTooltip = localize(
    "PresetSaveHint",
    "This preset is empty.\nHold 1 second to save your current checkboxes into it.",
  );

  return (
    <>
      <div className={styles.toolbar}>
        <div className={styles.toolbarLeft}>
          {/* [i] controls vanilla game tooltips only. */}
          <CwdTooltip tooltip={infoTooltip} alwaysVisible>
            <PanelButton
              tone={allTooltipsDisabled ? "danger" : "default"}
              ariaPressed={allTooltipsDisabled}
              iconSrc={infoIconSrc}
              onClick={() => { OnDisableAllTooltipsToggle(!allTooltipsDisabled); }}
            />
          </CwdTooltip>

          <CwdTooltip tooltip={roadNameTooltip}>
            <PanelButton
              tone={roadNamesHidden ? "active" : "default"}
              ariaPressed={roadNamesHidden}
              iconSrc={roadNameOnSrc}
              iconKind="map"
              onClick={() => { OnHideRoadNamesToggle(!roadNamesHidden); }}
            />
          </CwdTooltip>

          <CwdTooltip tooltip={roadArrowTooltip}>
            <PanelButton
              tone={roadArrowsShown ? "active" : "default"}
              ariaPressed={roadArrowsShown}
              iconSrc={roadArrowIconSrc}
              iconKind="map"
              onClick={() => { OnShowRoadArrowsToggle(!roadArrowsShown); }}
            />
          </CwdTooltip>

          <CwdTooltip tooltip={districtNameTooltip}>
            <PanelButton
              tone={districtNamesHidden ? "active" : "default"}
              ariaPressed={districtNamesHidden}
              iconSrc={districtIconSrc}
              iconKind="map"
              onClick={() => { OnHideDistrictNamesToggle(!districtNamesHidden); }}
            />
          </CwdTooltip>
        </div>

        <div className={styles.toolbarButtons}>
          <CwdTooltip tooltip={sortTooltip}>
            <PanelButton
              kind="sort"
              iconSrc={sortIconSrc}
              iconKind="sort"
              onClick={onSortButtonClick}
            />
          </CwdTooltip>

          <CwdTooltip
            tooltip={
              activeSort
                ? localize("BackToGrouped", "Back to grouped list")
                : allSectionsExpanded
                  ? localize("CollapseAll", "Collapse All Rows")
                  : localize("ExpandAll", "Expand All Rows")
            }
          >
            <PanelButton
              kind="count"
              tone={toggleAllTone}
              onClick={activeSort ? onExitToGroupedView : onToggleAllSections}
            >
              <PanelButtonText kind="count">
                {selectedTotalCount}/{totalNotificationCount}
              </PanelButtonText>
            </PanelButton>
          </CwdTooltip>

          {/* Presets save/load checkbox layouts. */}
          <div className={presetStyles.presetGroup}>
            <CwdTooltip tooltip={preset1Saved ? savedPresetTooltip : emptyPresetTooltip}>
              <PresetSlot
                label="1"
                saved={preset1Saved}
                active={activePreset === 1}
                onLoad={() => { OnLoadPreset(1); }}
                onSave={() => { OnSavePreset(1); }}
              />
            </CwdTooltip>

            <CwdTooltip tooltip={preset2Saved ? savedPresetTooltip : emptyPresetTooltip}>
              <PresetSlot
                label="2"
                saved={preset2Saved}
                active={activePreset === 2}
                onLoad={() => { OnLoadPreset(2); }}
                onSave={() => { OnSavePreset(2); }}
              />
            </CwdTooltip>
          </div>

          <CwdTooltip
            tooltip={localize(
              "ToggleAllTooltip",
              "Show or hide ALL map notification icons at once.\nColor: green = all shown; blue = mixed; red = all hidden.",
            )}
          >
            <PanelButton
              kind="toggle"
              tone={toggleAllTone}
              onClick={() => { setAllNotifications(!allSelected); }}
            >
              <PanelButtonText kind="toggle">
                {allSelected
                  ? localize("HideIcons", "Hide All")
                  : localize("ShowIcons", "Show All")}
              </PanelButtonText>
            </PanelButton>
          </CwdTooltip>
        </div>
      </div>

      <div className={styles.iconPreloader} aria-hidden="true">
        {preloadedIconSources.map((source) => (
          <img key={source} src={source} alt="" />
        ))}
      </div>
    </>
  );
};
