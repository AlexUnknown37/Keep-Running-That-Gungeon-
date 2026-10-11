# Keep Running — MODLOG

Small standalone BepInEx mod: Enter the Gungeon keeps running (and optionally keeps sound) when the window loses focus.
Split out of Gungeon Online at the user's request (2026-10-10). Same toolchain: `build.ps1` (auto minor-version bump,
references `..\Multiplayer\BepInEx\core`), `deploy.ps1` (→ Gale Bare profile `plugins\Keep_Running-dev`).
`..\Multiplayer\GungeonOnline\launch-test-copy.ps1` also deploys it.

## How the game pauses (found with Multiplayer\tools\il-dump.ps1)
- `GameManager.OnApplicationFocus(bool)`: on focus loss, if a player exists and not loading/paused/PreventPausing →
  `Pause()` + `GameStatsManager.Save()`; always stores `m_applicationHasFocus`. Only `set_PreventPausing` reads that field.
- Audio: `AkInitializer.OnApplicationFocus/Pause` → `AkSoundEngineController.OnApplicationFocus/Pause` →
  `ActivateAudio(bool)` → `AkSoundEngine.Suspend()` / `WakeupFromSuspend()`.
- The game never sets `Application.runInBackground` itself (only unused PlayMaker actions do).

## Implementation
- Prefix on `GameManager.OnApplicationFocus`: skip (store focus flag via reflection) unless PauseWhenUnfocused.
- Prefix on `AkSoundEngineController.ActivateAudio`: skip `false` calls unless MuteWhenUnfocused.
- `Application.runInBackground = true` while not pausing.
- C# 5 compiler: no `AccessTools.FieldRef` (ref returns) — use FieldInfo.

## Status
- 0.1.0 built and installed in Bare. Not yet tested in game.
