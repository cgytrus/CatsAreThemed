# Cats are Themed
A mod that adds custom themes support to [Cats are Liquid - A Better Place](https://store.steampowered.com/app/1188080)

## Usage
Put custom theme files in the world pack folder, they should have a `.theme` extension

The format is XML, example theme (i'm too lazy to document everything (there's *a lot* of stuff),
enter the [modding Discord server](https://discord.gg/puNrzd6eEp) or read the code yourself if you need help):
```xml
<?xml version="1.0" encoding="UTF-8"?>
<CustomTheme>
  <baseTheme>3</baseTheme>
  <themeOverrides>
    <bloom>6</bloom>
    <primaryColor>1</primaryColor>
  </themeOverrides>
  <valueOverrides>
    <primaryColor>
      <r>0.0</r>
      <g>1.0</g>
      <b>0.0</b>
      <a>0.8</a>
    </primaryColor>
    <bloom>
      <enabled>false</enabled>
      <intensity>6.9</intensity>
    </bloom>
    <chromaticAberration>
      <enabled>true</enabled>
      <intensity>3.4</intensity>
    </chromaticAberration>
    <vignette>
      <enabled>true</enabled>
      <color>
        <r>0.0</r>
        <g>0.0</g>
        <b>0.0</b>
        <a>0.5</a>
      </color>
      <intensity>0.8</intensity>
    </vignette>
  </valueOverrides>
</CustomTheme>
```

## Installation
1. Install [Cats are Liquid API](https://github.com/cgytrus/CalApi)
2. Install [Cats are Themed](https://github.com/cgytrus/CatsAreThemed/releases/latest)
   the same way as Cats are Online API

## Contributing
1. Clone the repository
2. Put the missing DLLs into CatsAreThemed/libs (for a more detailed explanation,
   follow the [Plugin development](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html)
   guide on the BepInEx wiki starting from Gathering DLL dependencies)
