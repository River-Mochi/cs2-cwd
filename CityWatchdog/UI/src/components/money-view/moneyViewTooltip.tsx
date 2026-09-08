// File: src/UI/src/components/money-view/moneyViewTooltip.tsx
// Purpose: Custom hover tooltip for the bottom-toolbar money field.

import { useValue } from "cs2/api";
import { economyBudget, toolbarBottom } from "cs2/bindings";
import { Unit, useLocalization, type Localization } from "cs2/l10n";
import { useText } from "../shared/localization";
import { Children, isValidElement, type CSSProperties, type ReactNode } from "react";
import {
    moneyTooltipFontScale$,
    moneyTooltipMode$,
    moneyViewMode$,
    moneyView$,
} from "../../bindings/bindings";
import styles from "./moneyView.module.scss";
import {
    formatTooltipMoneyViewValue,
    getDisplayWholeValue,
    getNumericValue,
    getSignedAmountTone,
    HOURS_PER_GAME_MONTH,
    MONEY_ICON,
    MONEY_TOOLTIP_MODE_COMPACT,
    MONEY_TOOLTIP_MODE_MINI,
    MONEY_VIEW_MODE_MONTHLY,
} from "./moneyViewShared";

export const MoneyViewTooltipContent = ({ baseContent }: { readonly baseContent: ReactNode }) => {
    const localization = useLocalization();
    const text = useText();

    const moneyViewEnabled = useValue(moneyView$);
    const moneyViewMode = useValue(moneyViewMode$);
    const moneyTooltipMode = useValue(moneyTooltipMode$);
    const moneyTooltipFontScale = useValue(moneyTooltipFontScale$);

    const hourlyNet = getNumericValue(useValue(toolbarBottom.moneyDelta$));
    const monthlyIncome = getNumericValue(useValue(economyBudget.totalIncome$));

    // Budget expenses are monthly and negative in the tooltip.
    const monthlyExpenses = -Math.abs(getNumericValue(useValue(economyBudget.totalExpenses$)));
    const monthlyNet = monthlyIncome + monthlyExpenses;
    const hourlyIncome = monthlyIncome / HOURS_PER_GAME_MONTH;
    const hourlyExpenses = monthlyExpenses / HOURS_PER_GAME_MONTH;

    if (!moneyViewEnabled) {
        return <>{baseContent}</>;
    }

    const mini = moneyTooltipMode === MONEY_TOOLTIP_MODE_MINI;
    const selectedUnit = moneyViewMode === MONEY_VIEW_MODE_MONTHLY
        ? Unit.IntegerPerMonth
        : Unit.IntegerPerHour;

    // Full + Mini show both units; Compact follows the toolbar unit.
    const showBothUnits = moneyTooltipMode !== MONEY_TOOLTIP_MODE_COMPACT;
    const tooltipClassName = getTooltipRowsClassName(moneyTooltipMode);
    const tooltipValueSize = getTooltipValueSize(moneyTooltipFontScale);
    const tooltipStyle = {
        "--moneyTooltipValueSizeFull": tooltipValueSize,
        "--moneyTooltipValueSizeCompact": tooltipValueSize,
        "--moneyTooltipValueSizeMini": tooltipValueSize,
    } as CSSProperties;

    return (
        <div className={tooltipClassName} style={tooltipStyle}>
            <div className={styles.tooltipTitle}>City Watchdog</div>

            {!mini && (
                <>
                    <MoneyViewTooltipGroup
                        localization={localization}
                        label={text("MoneyViewTooltipIncome", "Income:")}
                        hourlyValue={hourlyIncome}
                        monthlyValue={monthlyIncome}
                        selectedUnit={selectedUnit}
                        showBothUnits={showBothUnits}
                        mode={moneyTooltipMode}
                    />
                    <MoneyViewTooltipGroup
                        localization={localization}
                        label={text("MoneyViewTooltipExpenses", "Expenses:")}
                        hourlyValue={hourlyExpenses}
                        monthlyValue={monthlyExpenses}
                        selectedUnit={selectedUnit}
                        showBothUnits={showBothUnits}
                        mode={moneyTooltipMode}
                    />
                    <div className={styles.tooltipDivider} />
                </>
            )}

            <MoneyViewTooltipGroup
                localization={localization}
                label={text("MoneyViewTooltipNet", "Net:")}
                hourlyValue={hourlyNet}
                monthlyValue={monthlyNet}
                selectedUnit={selectedUnit}
                showBothUnits={showBothUnits}
                mode={moneyTooltipMode}
            />
        </div>
    );
};

const getTooltipValueSize = (value: number): string => {
    const percent = Math.min(130, Math.max(90, Number(value) || 100));
    return `${percent / 100}em`;
};

const getTooltipRowsClassName = (mode: number): string => {
    const classes = [styles.tooltipRows];

    if (mode === MONEY_TOOLTIP_MODE_MINI) {
        classes.push(styles.tooltipRowsMini);
    } else if (mode === MONEY_TOOLTIP_MODE_COMPACT) {
        classes.push(styles.tooltipRowsCompact);
    } else {
        classes.push(styles.tooltipRowsFull);
    }

    return classes.join(" ");
};

export const isMoneyTooltip = (props: any): boolean => {
    return Boolean(props?.content) && containsIcon(props?.children, MONEY_ICON);
};

// Identify the vanilla money tooltip by its icon, not generated CSS names.
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

const MoneyViewTooltipGroup = ({
    localization,
    label,
    hourlyValue,
    monthlyValue,
    selectedUnit,
    showBothUnits,
    mode,
}: {
    readonly localization: Localization;
    readonly label: string;
    readonly hourlyValue: number;
    readonly monthlyValue: number;
    readonly selectedUnit: Unit;
    readonly showBothUnits: boolean;
    readonly mode: number;
}) => {
    return (
        <div className={styles.tooltipGroup}>
            <div className={styles.tooltipLabel}>{trimLabelPunctuation(label)}</div>
            <div className={styles.tooltipValueColumn}>
                {showBothUnits ? (
                    <>
                        <MoneyViewTooltipValue
                            localization={localization}
                            value={hourlyValue}
                            unit={Unit.IntegerPerHour}
                            mode={mode}
                        />
                        <MoneyViewTooltipValue
                            localization={localization}
                            value={monthlyValue}
                            unit={Unit.IntegerPerMonth}
                            mode={mode}
                        />
                    </>
                ) : (
                    <MoneyViewTooltipValue
                        localization={localization}
                        value={selectedUnit === Unit.IntegerPerMonth ? monthlyValue : hourlyValue}
                        unit={selectedUnit}
                        mode={mode}
                    />
                )}
            </div>
        </div>
    );
};

const MoneyViewTooltipValue = ({
    localization,
    value,
    unit,
    mode,
}: {
    readonly localization: Localization;
    readonly value: number;
    readonly unit: Unit;
    readonly mode: number;
}) => {
    const displayValue = getDisplayWholeValue(value);
    const tone = getSignedAmountTone(displayValue);

    // All tooltip modes keep the short M/B formatting for large values.
    const formattedValue = formatTooltipMoneyViewValue(localization, displayValue, true, unit);

    return (
        <div className={`${styles.tooltipValueLine} ${getTooltipValueClassName(mode)} ${styles[tone]}`}>
            {formattedValue}
        </div>
    );
};

const getTooltipValueClassName = (mode: number): string => {
    if (mode === MONEY_TOOLTIP_MODE_MINI) {
        return styles.tooltipValueLineMini;
    }

    if (mode === MONEY_TOOLTIP_MODE_COMPACT) {
        return styles.tooltipValueLineCompact;
    }

    return styles.tooltipValueLineFull;
};

const trimLabelPunctuation = (label: string): string => {
    return label.replace(/[\s:：]+$/u, "");
};
