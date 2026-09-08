// File: src/UI/src/components/panel/notification-panel/notificationPanel.tsx
// Purpose: In-city CWD notification panel state and row lists.

import { useValue } from "cs2/api";
import { game } from "cs2/bindings";
import { useLocalization } from "cs2/l10n";
import { useCallback, useEffect, useMemo, useState } from "react";
import {
  controlPanelEnabled$,
  miniHudFavorites$,
  notificationCounts$,
  panelButtonsOnlyStart$,
  panelCollapsedSectionsMask$,
  panelPositionX$,
  panelPositionY$,
  panelSortMode$,
  OnControlPanelBindingToggle,
  OnPanelCollapsedSectionsChanged,
  OnPanelSortModeChanged,
  OnToggleMiniHudFavorite,
} from "../../../bindings/bindings";
import { useText } from "../../shared/localization";
import { NotificationRow } from "../notification-row/notificationRow";
import SortActivePath from "../../../../images/sort-active.svg";
import SortArrowDownPath from "../../../../images/sort-arrow-down.svg";
import SortArrowUpPath from "../../../../images/sort-arrow-up.svg";
import styles from "./notificationPanel.module.scss";
import {
  allItems,
  collapsedSectionsMask,
  createExpandedSections,
  expandedSectionsFromMask,
  sections,
  type Localize,
  type NotificationSection,
} from "./notificationData";
import { DraggablePanelFrame } from "./notificationPanelFrame";
import { useAllNotificationValues } from "./notificationHooks";
import { NotificationPanelToolbar } from "./notificationPanelToolbar";
import { NotificationSectionView } from "./notificationSectionView";

const SORT_ASCENDING = 0;
const SORT_DESCENDING = 1;
const SORT_ACTIVE = 2;

// Keep the chosen sort while the panel closes/reopens this session.
let sessionSortMode = SORT_ASCENDING;

export const NotificationPanel = () => {
  const showPanel = useValue(controlPanelEnabled$);
  const isPhotoMode = useValue(game.activeGamePanel$)?.__Type == game.GamePanelType.PhotoMode;

  if (isPhotoMode || !showPanel) {
    return null;
  }

  return <NotificationPanelContent />;
};

const NotificationPanelContent = () => {
  const localization = useLocalization();
  const uiText = useText();
  const { translate } = localization;

  const [sortMode, setSortMode] = useState(sessionSortMode);
  const [activeSnapshot, setActiveSnapshot] = useState<number[] | null>(null);
  const panelButtonsOnlyStart = useValue(panelButtonsOnlyStart$);
  const [panelCollapsed, setPanelCollapsed] = useState(() => panelButtonsOnlyStart);
  const [expandedSections, setExpandedSections] = useState(createExpandedSections);

  const allValues = useAllNotificationValues();
  const notificationCounts = useValue(notificationCounts$);
  const miniHudFavorites = useValue(miniHudFavorites$);
  const favoriteIndexes = useMemo(() => new Set(miniHudFavorites), [miniHudFavorites]);

  const savedPanelPositionX = useValue(panelPositionX$);
  const savedPanelPositionY = useValue(panelPositionY$);
  const savedCollapsedMask = useValue(panelCollapsedSectionsMask$);
  const savedSortMode = useValue(panelSortMode$);

  // Active view freezes counts so rows don't jump while reading.
  useEffect(() => {
    if (sortMode !== SORT_ACTIVE) {
      return;
    }

    if (activeSnapshot === null && notificationCounts.length > 0) {
      setActiveSnapshot(notificationCounts.slice());
    }
  }, [sortMode, activeSnapshot, notificationCounts]);

  useEffect(() => {
    setExpandedSections(expandedSectionsFromMask(savedCollapsedMask));
  }, [savedCollapsedMask]);

  useEffect(() => {
    setSortMode(savedSortMode);
    sessionSortMode = savedSortMode;
  }, [savedSortMode]);

  const setAndSaveSortMode = (mode: number) => {
    setSortMode(mode);
    sessionSortMode = mode;
    OnPanelSortModeChanged(mode);
    setActiveSnapshot(
      mode === SORT_ACTIVE && notificationCounts.length > 0
        ? notificationCounts.slice()
        : null,
    );
  };

  const cycleSortMode = () => setAndSaveSortMode((sortMode + 1) % 3);
  const exitToGroupedView = () => setAndSaveSortMode(SORT_ASCENDING);

  // arrayIndex = local checkbox state. countIndex = stable C# count/jump index.
  const seenActiveIdentities = new Set<string>();
  const activeRows = sortMode === SORT_ACTIVE
    ? allItems
      .map((item, arrayIndex) => ({
        item,
        arrayIndex,
        count: activeSnapshot?.[item.countIndex] ?? 0,
      }))
      .filter((entry) => entry.count > 0 && !entry.item.optional)
      .sort((a, b) => b.count - a.count || a.item.countIndex - b.item.countIndex)
      .filter((entry) => {
        // Shared alerts only show once in the flat Active list.
        const identity = entry.item.miniHudIdentity ?? entry.item.localeId;
        if (seenActiveIdentities.has(identity)) {
          return false;
        }

        seenActiveIdentities.add(identity);
        return true;
      })
    : [];

  // Optional rows don't affect the bulk Show/Hide state.
  const toggleAllValues = allItems
    .map((item, arrayIndex) => ({ item, value: allValues[arrayIndex] ?? false }))
    .filter((entry) => !entry.item.optional)
    .map((entry) => entry.value);

  const allSelected = toggleAllValues.every(Boolean);
  const anySelected = toggleAllValues.some(Boolean);
  const selectedTotalCount = toggleAllValues.filter(Boolean).length;
  const totalNotificationCount = toggleAllValues.length;
  const toggleAllTone =
    allSelected
      ? "on" as const
      : anySelected
        ? "partial" as const
        : "off" as const;

  const allSectionsExpanded = sections.every(
    (section) => expandedSections[section.localeId] === true,
  );

  const sortIconSrc =
    sortMode === SORT_ASCENDING
      ? SortArrowUpPath
      : sortMode === SORT_DESCENDING
        ? SortArrowDownPath
        : SortActivePath;

  const localize: Localize = useCallback((localeId, fallback, raw = false) => {
    if (raw) {
      return translate(localeId) ?? fallback ?? localeId;
    }

    return uiText(localeId, fallback);
  }, [translate, uiText]);

  const sortTooltip =
    sortMode === SORT_ASCENDING
      ? localize("SortModeAscending", "Sorting: A → Z · click to cycle")
      : sortMode === SORT_DESCENDING
        ? localize("SortModeDescending", "Sorting: Z → A · click to cycle")
        : localize("SortModeActiveFirst", "Sorting: active alerts first · click to cycle");

  const panelTitle =
    sortMode === SORT_ACTIVE
      ? localize("PanelTitleActiveAlerts", "ACTIVE ALERTS")
      : "CITY WATCHDOG";

  const orderedSections = useMemo(
    () => [...sections].sort((a, b) => {
      const result = localize(a.localeId).localeCompare(localize(b.localeId));
      return sortMode === SORT_DESCENDING ? -result : result;
    }),
    [sortMode, localize],
  );

  const applyExpandedSections = (next: Record<string, boolean>) => {
    setExpandedSections(next);
    OnPanelCollapsedSectionsChanged(collapsedSectionsMask(next));
  };

  const onToggleAllSections = () => {
    if (panelCollapsed) {
      setPanelCollapsed(false);
      applyExpandedSections(createExpandedSections(true));
      return;
    }

    applyExpandedSections(createExpandedSections(!allSectionsExpanded));
  };

  // Collapsed Sort opens the current view instead of cycling it unseen.
  const onSortButtonClick = () => {
    if (!panelCollapsed) {
      cycleSortMode();
      return;
    }

    setPanelCollapsed(false);

    if (sortMode === SORT_ACTIVE) {
      if (notificationCounts.length > 0) {
        setActiveSnapshot(notificationCounts.slice());
      }
      return;
    }

    applyExpandedSections(createExpandedSections(true));
  };

  const onSectionExpandedChange = (
    section: NotificationSection,
    expanded: boolean,
  ) => {
    applyExpandedSections({
      ...expandedSections,
      [section.localeId]: expanded,
    });
  };

  const onPanelCollapsedToggle = () => {
    const collapsing = !panelCollapsed;
    setPanelCollapsed(collapsing);

    // Reopen Active with fresh counts.
    if (
      !collapsing &&
      sortMode === SORT_ACTIVE &&
      notificationCounts.length > 0
    ) {
      setActiveSnapshot(notificationCounts.slice());
    }
  };

  return (
    <DraggablePanelFrame
      savedOffset={{
        x: savedPanelPositionX,
        y: savedPanelPositionY,
      }}
      localize={localize}
      panelTitle={panelTitle}
      panelCollapsed={panelCollapsed}
      allSectionsExpanded={allSectionsExpanded}
      onPanelCollapsedToggle={onPanelCollapsedToggle}
      onCloseClick={() => { OnControlPanelBindingToggle(false); }}
    >
      <NotificationPanelToolbar
        localize={localize}
        sortIconSrc={sortIconSrc}
        sortTooltip={sortTooltip}
        activeSort={sortMode === SORT_ACTIVE}
        allSectionsExpanded={allSectionsExpanded}
        selectedTotalCount={selectedTotalCount}
        totalNotificationCount={totalNotificationCount}
        toggleAllTone={toggleAllTone}
        allSelected={allSelected}
        onSortButtonClick={onSortButtonClick}
        onExitToGroupedView={exitToGroupedView}
        onToggleAllSections={onToggleAllSections}
      />

      {/* Active = flat frozen list; grouped modes keep sections. */}
      {!panelCollapsed && sortMode === SORT_ACTIVE && (
        activeRows.length === 0
          ? (
            <div
              style={{
                paddingTop: "12rem",
                paddingBottom: "12rem",
                textAlign: "center",
                opacity: 0.6,
              }}
            >
              {localize("NoActiveAlerts", "No active notifications.")}
            </div>
          )
          : activeRows.map(({ item, arrayIndex }) => (
            <NotificationRow
              key={item.localeId}
              item={item}
              isChecked={allValues[arrayIndex] ?? false}
              count={notificationCounts[item.countIndex] ?? 0}
              favorite={favoriteIndexes.has(item.countIndex)}
              onFavoriteToggle={() => OnToggleMiniHudFavorite(item.countIndex)}
              localize={localize}
            />
          ))
      )}

      {!panelCollapsed &&
        sortMode !== SORT_ACTIVE &&
        orderedSections.map((section, index) => (
          <NotificationSectionView
            key={section.localeId}
            section={section}
            expanded={expandedSections[section.localeId] === true}
            localize={localize}
            notificationCounts={notificationCounts}
            favoriteIndexes={favoriteIndexes}
            showDivider={index > 0}
            onExpandedChange={(expanded) => {
              onSectionExpandedChange(section, expanded);
            }}
          />
        ))}
    </DraggablePanelFrame>
  );
};
