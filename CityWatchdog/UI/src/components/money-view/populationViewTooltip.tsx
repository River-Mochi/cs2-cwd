// File: src/UI/src/components/money-view/populationViewTooltip.tsx
// Purpose: Adds CWD population flow rows to the vanilla population tooltip.

import { bindValue, useValue } from "cs2/api";
import { infoview, toolbarBottom } from "cs2/bindings";
import { LocalizedNumber, Unit, useLocalization, type Localization } from "cs2/l10n";
import { useText } from "../shared/localization";
import { Children, isValidElement, type CSSProperties, type ReactNode } from "react";
import { moneyViewMode$, moneyView$, populationTooltipFontScale$ } from "../../bindings/bindings";
import styles from "./moneyView.module.scss";
import {
  getDisplayWholeValue,
  getNumericValue,
  getSignedAmountTone,
  HOURS_PER_GAME_MONTH,
  MONEY_VIEW_MODE_MONTHLY,
  POPULATION_ICON,
} from "./moneyViewShared";

// Vanilla population data only — no CWD sim queries.
const homeless$ = bindValue<number>("populationInfo", "homeless", 0); // not in generated bindings yet
const homelessness$ = bindValue<number>("populationInfo", "homelessness", 0); // game-calculated homeless %

export const PopulationViewTooltipContent = ({ baseContent }: { readonly baseContent: ReactNode }) => {
  const localization = useLocalization();
  const text = useText();

  const moneyViewEnabled = useValue(moneyView$);
  const moneyViewMode = useValue(moneyViewMode$);
  const populationTooltipFontScale = useValue(populationTooltipFontScale$);
  const unemployment = getNumericValue(useValue(infoview.unemployment$)); // generated vanilla binding

  // Toolbar trend is hourly; tooltip shows the opposite unit.
  const populationDelta = getNumericValue(useValue(toolbarBottom.populationDelta$));
  const trendValue = moneyViewMode === MONEY_VIEW_MODE_MONTHLY
    ? populationDelta
    : populationDelta * HOURS_PER_GAME_MONTH;
  const trendUnit = moneyViewMode === MONEY_VIEW_MODE_MONTHLY
    ? Unit.IntegerPerHour
    : Unit.IntegerPerMonth;

  const births = getNumericValue(useValue(infoview.birthRate$));
  const deaths = getNumericValue(useValue(infoview.deathRate$));
  const homeless = getNumericValue(useValue(homeless$));
  const homelessRate = getNumericValue(useValue(homelessness$));
  const movedIn = getNumericValue(useValue(infoview.movedIn$));
  const movedAway = getNumericValue(useValue(infoview.movedAway$));

  if (!moneyViewEnabled) {
    return baseContent ? <>{baseContent}</> : null;
  }

  const tooltipStyle = {
    "--populationTooltipValueSize": getTooltipValueSize(populationTooltipFontScale),
  } as CSSProperties;

  return (
    <div className={styles.populationTooltipWrapper} style={tooltipStyle}>
      <div className={styles.tooltipTitle}>City Watchdog</div>

      <PopulationTooltipUnemployment
        localization={localization}
        label={text("PopulationTooltipUnemployment", "Unemployment:")}
        value={unemployment}
      />
      <PopulationTooltipRate
        localization={localization}
        label={text("PopulationTooltipCurrentTrend", "Trend:")}
        value={getDisplayWholeValue(trendValue)}
        unit={trendUnit}
      />

      <div className={styles.populationTooltipExtra}>
        <PopulationTooltipFlow
          localization={localization}
          label={text("PopulationTooltipBirths", "Births:")}
          value={births}
          direction={1}
        />
        <PopulationTooltipFlow
          localization={localization}
          label={text("PopulationTooltipDeaths", "Deaths:")}
          value={deaths}
          direction={-1}
        />
        <PopulationTooltipFlow
          localization={localization}
          label={text("PopulationTooltipMovedIn", "Moved in:")}
          value={movedIn}
          direction={1}
        />
        <PopulationTooltipFlow
          localization={localization}
          label={text("PopulationTooltipMovedOut", "Moved out:")}
          value={movedAway}
          direction={-1}
        />
        <PopulationTooltipHomeless
          localization={localization}
          label={text("PopulationTooltipHomeless", "Homeless:")}
          count={homeless}
          rate={homelessRate}
        />
      </div>
    </div>
  );
};

export const isPopulationTooltip = (props: any): boolean => {
  return containsIcon(props?.children, POPULATION_ICON);
};

// Walk vanilla tooltip tree instead of relying on generated CSS names.
const containsIcon = (node: ReactNode, icon: string): boolean => {
  if (!isValidElement(node)) {
    return false;
  }

  const props = node.props as any;
  if (props?.icon === icon) {
    return true;
  }

  return Children.toArray(props?.children).some((child) => containsIcon(child, icon));
};

const PopulationTooltipFlow = ({
  localization,
  label,
  value,
  direction,
}: {
  readonly localization: Localization;
  readonly label: string;
  readonly value: number;
  readonly direction: 1 | -1;
}) => {
  const displayValue = getDisplayWholeValue(value);

  // Births/moved-in add; deaths/moved-out subtract.
  const signedValue = displayValue === 0 ? 0 : displayValue * direction;

  return (
    <PopulationTooltipRate
      localization={localization}
      label={label}
      value={signedValue}
      unit={Unit.IntegerPerMonth}
    />
  );
};

const PopulationTooltipHomeless = ({
  localization,
  label,
  count,
  rate,
}: {
  readonly localization: Localization;
  readonly label: string;
  readonly count: number;
  readonly rate: number;
}) => {
  // Show count + vanilla rate, e.g. 524 (0.5%).
  const countText = formatLocalizedIntegerValue(localization, getDisplayWholeValue(count), Unit.Integer);
  const rateText = formatLocalizedIntegerValue(localization, rate, Unit.PercentageSingleFraction);

  return (
    <div className={styles.populationTooltipGroup}>
      <div className={styles.tooltipLabel}>{trimLabelPunctuation(label)}</div>
      <div className={`${styles.populationTooltipValueLine} ${styles.softNeutral}`}>
        {`${countText} (${rateText})`}
      </div>
    </div>
  );
};

const PopulationTooltipUnemployment = ({
  localization,
  label,
  value,
}: {
  readonly localization: Localization;
  readonly label: string;
  readonly value: number;
}) => {
  // Level, not a +/- flow.
  return (
    <PopulationTooltipRate
      localization={localization}
      label={label}
      value={value}
      unit={Unit.PercentageSingleFraction}
      topRow={true}
      toneOverride="softNeutral"
      showSign={false}
    />
  );
};

const PopulationTooltipRate = ({
  localization,
  label,
  value,
  unit,
  topRow = false,
  toneOverride,
  showSign = true,
}: {
  readonly localization: Localization;
  readonly label: string;
  readonly value: number;
  readonly unit: Unit;
  readonly topRow?: boolean;
  readonly toneOverride?: "positive" | "negative" | "neutral" | "softNeutral";
  readonly showSign?: boolean;
}) => {
  const tone = toneOverride ?? getSignedAmountTone(value);
  const formattedValue = showSign
    ? formatPopulationRateValue(localization, value, unit)
    : formatLocalizedIntegerValue(localization, value, unit);

  return (
    <div className={`${styles.populationTooltipGroup} ${topRow ? styles.populationTooltipTopTrend : ""}`}>
      <div className={styles.tooltipLabel}>{trimLabelPunctuation(label)}</div>
      <div className={`${styles.populationTooltipValueLine} ${styles[tone]}`}>{formattedValue}</div>
    </div>
  );
};

const formatPopulationRateValue = (localization: Localization, value: number, unit: Unit): string => {
  const magnitude = formatLocalizedIntegerValue(localization, Math.abs(value), unit);
  const spacer = "\u200A";

  if (value > 0) {
    return `+${spacer}${magnitude}`;
  }

  if (value < 0) {
    return `-${spacer}${magnitude}`;
  }

  return magnitude;
};

const formatLocalizedIntegerValue = (localization: Localization, value: number, unit: Unit): string => {
  try {
    // vanilla formatting for separators + rate suffixes.
    return LocalizedNumber.renderString(localization, {
      value,
      unit,
      signed: false,
    });
  } catch {
    const suffix =
      unit === Unit.IntegerPerMonth
        ? " /mo"
        : unit === Unit.IntegerPerHour
          ? " /h"
          : "";

    return `${Math.round(Math.abs(value)).toString()}${suffix}`;
  }
};

const getTooltipValueSize = (value: number): string => {
  const percent = Math.min(130, Math.max(90, Number(value) || 100));
  return `${percent / 100}em`;
};

const trimLabelPunctuation = (label: string): string => {
  return label.replace(/[\s:：]+$/u, "");
};
