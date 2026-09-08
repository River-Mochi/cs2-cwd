// File: src/UI/src/components/panel/notification-panel/notificationSectionView.tsx
// Purpose: One grouped notification section in the CWD panel.

import { memo, useMemo } from "react";
import { OnToggleMiniHudFavorite } from "../../../bindings/bindings";
import { Divider } from "../../divider/divider";
import { InfoPanel } from "../info-panel/infoPanel";
import { NotificationRow } from "../notification-row/notificationRow";
import {
  notificationCountIndexes,
  type Localize,
  type NotificationSection,
} from "./notificationData";
import { useSectionValues } from "./notificationHooks";

export const NotificationSectionView = memo(({
  section,
  expanded,
  localize,
  notificationCounts,
  favoriteIndexes,
  showDivider,
  onExpandedChange,
}: {
  section: NotificationSection;
  expanded: boolean;
  localize: Localize;
  notificationCounts: number[];
  favoriteIndexes: Set<number>;
  showDivider: boolean;
  onExpandedChange: (expanded: boolean) => void;
}) => {
  const values = useSectionValues(section);
  const selectedCount = values.filter(Boolean).length;

  // Rows stay A→Z even when section groups are Z→A.
  const orderedItems = useMemo(
    () => section.items
      .map((item, itemIndex) => ({ item, itemIndex }))
      .sort((a, b) => {
        const firstLabel = localize(a.item.localeId).toLocaleLowerCase();
        const secondLabel = localize(b.item.localeId).toLocaleLowerCase();
        const result = firstLabel.localeCompare(secondLabel);
        return result || a.itemIndex - b.itemIndex;
      }),
    [section, localize],
  );

  const summaryState =
    selectedCount === section.items.length
      ? "on"
      : selectedCount > 0
        ? "partial"
        : "off";

  return (
    <>
      {showDivider && <Divider />}
      <InfoPanel
        title={localize(section.localeId)}
        collapsible={true}
        expanded={expanded}
        onExpandedChange={onExpandedChange}
        summary={`${selectedCount}/${section.items.length}`}
        summaryState={summaryState}
        renderChildren={() => orderedItems.map(({ item, itemIndex }) => {
          const countIndex = notificationCountIndexes.get(item.localeId) ?? -1;

          return (
            <NotificationRow
              key={item.localeId}
              item={item}
              isChecked={values[itemIndex] ?? false}
              count={notificationCounts[countIndex] ?? 0}
              favorite={favoriteIndexes.has(countIndex)}
              onFavoriteToggle={() => OnToggleMiniHudFavorite(countIndex)}
              localize={localize}
            />
          );
        })}
      />
    </>
  );
}, (prev, next) =>
  prev.section === next.section &&
  prev.expanded === next.expanded &&
  prev.localize === next.localize &&
  prev.notificationCounts === next.notificationCounts &&
  prev.favoriteIndexes === next.favoriteIndexes &&
  prev.showDivider === next.showDivider);
