// File: src/UI/src/index.tsx
// Purpose: Registers City Watchdog UI modules and vanilla UI extensions.

import { ModRegistrar, type ModuleRegistry, type ModuleRegistryExtend } from "cs2/modding";
import mod from "../mod.json";
import { EntryButton } from "./components/entry-button/entryButton";
import { MiniHud } from "./components/mini-hud/miniHud";
import { NotificationPanel } from "./components/panel/notification-panel/notificationPanel";
import { EditorQuickControls } from "./components/editor-quick-controls/editorQuickControls";
import { DescriptionTooltipMoneyViewExtension, StatFieldMoneyViewExtension, TooltipGateExtension } from "./components/money-view/moneyView";
import { VanillaComponentResolver } from "./utils/vanilla";
// Side-effect import — webpack emits this SVG to coui://ui-mods/images/ so the C# Options UI
// Settings can reference it by raw path string (no TS importer for it on the C# side).
import "../images/NotificationIcon_PawRainbow.svg";

const STAT_FIELD_MODULE = "game-ui/game/components/toolbar/components/stat-field/stat-field.tsx";
// Vanilla export name. Keep this value aligned with the game module export.
const STAT_FIELD_VANILLA_TREND_EXPORT = "StatFieldTrend";
const TOOLTIP_MODULE = "game-ui/common/tooltip/tooltip.tsx";
const TOOLTIP_EXPORT = "Tooltip";
const DESCRIPTION_TOOLTIP_MODULE = "game-ui/common/tooltip/description-tooltip/description-tooltip.tsx";
const DESCRIPTION_TOOLTIP_EXPORT = "DescriptionTooltip";

const extendSafe = (
    registry: ModuleRegistry,
    modulePath: string,
    exportId: string,
    extension: ModuleRegistryExtend
) => {
    try {
        registry.extend(modulePath, exportId, extension);
    } catch (error) {
        console.error(`${mod.id} - UI extension failed for ${modulePath}#${exportId}`, error);
    }
};

const register: ModRegistrar = (moduleRegistry) => {
    VanillaComponentResolver.setRegistry(moduleRegistry);
    extendSafe(moduleRegistry, STAT_FIELD_MODULE, STAT_FIELD_VANILLA_TREND_EXPORT, StatFieldMoneyViewExtension);
    extendSafe(moduleRegistry, TOOLTIP_MODULE, TOOLTIP_EXPORT, TooltipGateExtension);
    extendSafe(moduleRegistry, DESCRIPTION_TOOLTIP_MODULE, DESCRIPTION_TOOLTIP_EXPORT, DescriptionTooltipMoneyViewExtension);
    moduleRegistry.append("GameTopLeft", EntryButton);
    moduleRegistry.append("Game", NotificationPanel);
    moduleRegistry.append("Game", MiniHud);
    moduleRegistry.append("Editor", EditorQuickControls);
};

export default register;
