# Keep Running That Gungeon

A tiny BepInEx mod that stops **Enter the Gungeon** from pausing when you tab out of the game.
Handy for co-op, streaming, or playing with a second monitor.

> A small **side project** (spun off from [Gungeon Online](https://github.com/AlexUnknown37/GungeonOnline)).
> Early version, lightly tested. No support or guarantees.

## Settings

In `BepInEx/config/keeprunning.etg.cfg` (or your mod manager's config editor):

- `PauseWhenUnfocused` (default **false**): set to `true` to get the normal pause-on-tab-out back.
- `MuteWhenUnfocused` (default **true**): set to `false` to keep hearing the game in the background.

Restart the game after changing a setting. Only the automatic pause on tab-out is removed; the pause menu works normally.

## Requirements

- Enter the Gungeon
- [BepInExPack for EtG](https://thunderstore.io/c/enter-the-gungeon/) (a mod manager like Gale installs it)

## Building

Plain C# 5 compiled with the C# compiler that ships with Windows (no SDK needed).
`build.ps1` builds `KeepRunning.dll` and a Thunderstore-style zip in `dist/`. It expects the game's
`Managed` folder and a BepInEx `core` folder at the paths at the top of the script; adjust them for your machine.

## Credits

- [BepInEx](https://github.com/BepInEx/BepInEx) and HarmonyX
- Built with help from Claude (Anthropic), an AI assistant

## License

MIT

## Support

If you enjoy this side project, you can support me on Ko-fi: https://ko-fi.com/alexjustalex
