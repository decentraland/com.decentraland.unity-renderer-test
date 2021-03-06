### How to profile a local Unity build remotely

#### Option 1

1. Run `make watch` in a terminal at your `kernel` repository. Wait for it to finish and leave it watching for changes.
2. In Unity Editor, build unity WASM with its name as `unity` into your desired folder. **It's very important that the folder/build name is `unity`**.
3. After that, run the bundling process in `unity-renderer` by executing `npm run build` in the `./browser-interface` folder.
4. When the building process finishes, move all the "files" files that will end up inside `browser-interface/dist` into `kernel/static/unity-renderer`.

Just to be clear, the generated files should end up in the following path

```
kernel
 /- static
    /- unity-renderer
      /- index.js
      /- unity.data.unityweb
      /- unity.wasm.code.unityweb
      /- unity.wasm.framework.unityweb
      /- ...
```

#### Option 2

1. Run `make watch` in a terminal at `kernel/`. Wait for it to finish and leave it watching for changes. **This step must be done before replacing the build files or they may be replaced again with the latest published build files when the `make watch` process runs**
2. Build unity WASM with its name as `unity` into the folder `kernel/static`. **It's very important that the folder/build name is `unity`**.
3. Testing how your new build performs:

- Open **[http://localhost:3000/?DEBUG_MODE&LOCAL_COMMS&position=-100,100](http://localhost:3000/?DEBUG_MODE&LOCAL_COMMS&position=-100,100)** to go to an area with a high density of test parcels.
- Open **[http://localhost:3000/?DEBUG_MODE&LOCAL_COMMS&ENV=org&position=10,0](http://localhost:3000/?DEBUG_MODE&LOCAL_COMMS&ENV=org&position=10,0)** to open an area with real-life deployments (but without communicating with other users).
- Open **[http://localhost:3000/?ENV=org&position=0,90](http://localhost:3000/?ENV=org&position=0,90)** to open the explorer near the Decentraland Museum

### Profiling a build with Unity Editor

1. In Unity Editor open the profiler at window -> analysis -> profiler
2. Open the build settings and toggle on "Development Build", "Autoconnect Profiler" and **make sure Deep Profiling is toggled off** before starting the build
3. Follow the steps at this readme's "Making a manual build" section
4. Open the new build (http://localhost:3000/?ENV=org) and after starting the explorer your Profiler window at Unity Editor should start receiving and displaying the data

#### Important notes and recommendations

- The same Unity Editor instance that made the build **shouldn't be closed**, as that will make the new build useless for profiling, at least at the time of writting this, **it only sends the data to the same Unity Editor instance that built it**.

- Since the profiler data comes from the build, you can't "pause the game" and analyze the data, that should be done toggling the "record" button, keep in mind that it doesn't work as expected when re-toggled, and you probably have to toggle/untoggle it several times to start receiving the data again, in the same session in which the button was untoggled to analyze the data.

- Recommended setup: Only CPU, Rendering and Memory modules on display; Disable VSync and "Others" from CPU module.
