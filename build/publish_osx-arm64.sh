# Publish Application
dotnet publish ../src/TiktokLiveRec.macOS/TiktokLiveRec.macOS.csproj -c Release -r osx-arm64

# Create .app structure
mkdir -p TiktokLiveRec.app/Contents/{MacOS,Resources}

# Copy executable files
cp -r ../src/TikTokLiveRec.macOS/bin/Release/net9.0/osx-arm64/publish/* TiktokLiveRec.app/Contents/MacOS/

# Copy Info.plist from project directory
cp ./Info.plist TiktokLiveRec.app/Contents/

# Copy icon file (if exists)
cp ./Favicon.icns TiktokLiveRec.app/Contents/Resources/

# Set executable permissions
chmod +x TiktokLiveRec.app/Contents/MacOS/TiktokLiveRec

# Create temporary directory
mkdir -p dist

# Copy .app to temporary directory
cp -r TiktokLiveRec.app dist/

# Create .dmg file
create-dmg \
    --window-size 660 400 \
    --icon-size 120 \
    --icon TiktokLiveRec.app 165 175 \
    --app-drop-link 495 175 \
    TiktokLiveRec-arm64.dmg dist/

# Clean up temporary directory
rm -rf dist