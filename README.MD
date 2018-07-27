# goedle.io Unity SDK

We now give a brief introduction how easy our plugin for Unity can be implemented. We created a developer friendly SDK and the basic setup just takes about 2 minutes.

---

To get an APP and API key please signup on [goedle.io](http://www.goedle.io).

---

1st step - Add the goedle_io Unity Package as Asset ([download](http://www.goedle.io/unity/goedle_io_sdk.unitypackage)).

![Add the goedle.io Unity Packages manual](http://www.goedle.io/unity/1-goedle_add_asset.png "Add the goedle.io Unity Packages manual")


2nd step - Add “Goedle Loader" to your first scene. 

3rd and last step - You have to fill your credentials, APP and API key in.

![Add App and API key](http://www.goedle.io/unity/3-goedle_credentials.png "Add APP and API key")


Afterward, you are ready to start and add the tracking points all over neuralgic positions in your Unity app.
## Tracking Notation

To track data from a Unity application with the goedle.io SDK, the tracking points have to be set in the source code of the Unity application. Therefore, a certain tracking notation is recommended. Besides the event tracking functionality, the SDK holds additional functions, to respect the privacy and to identify a user. 

### Event
There are two main variables to track an event. The first is the action, e.g., view, share, like these variables are the specific actions which are done by a user. The second variable specifies the object of the action e.g., task, question, scene. These two variables are concatenated and delimited with a ".". Note however that a specifier is not mandatory and we also support hierarchies with more than two levels. The tracking origins in the Object-Notation framework, which also used by Segment or Google Analytics.

### Event ID
Furthermore, a event can be specified with an event id. As an example, if one wants to track the view-event of a scene, the event would be view, the specifier would be the scene and the event id the scene id X235123
### Event Value
If one has to track the information of the duration of a certain task or the result of a answered question or if the event has a certain value, an additional parameter can be passed. The event_value. The event name would be finished.task, the event id could be task-12 and the event value 69. Where 69 would be the duration in seconds.

### Game State
For certain behavioral analysis passive game changes have to be tracked. These can be seen as a stream of data which represent the value state of the Unity application. A passive game change is the adjusting of values which are done by the application automatically, depending on the game design. To identify a game state, the event should be called “game.state”, the event id is in this case, is the passively changed variable. And the event value, the value of the variable.

## Tracking Functions
To call the track function, the goedle.io SDK has to be integrated in the header of a Unity/ C# class file.
This can be achieved through:
```
using goedle_sdk;
```
There are three different tracking signatures which cover the in Section 4.1 described tracking notations, while the `game.state` has the same signature as the event value tracking call.

**Tracking a single event**

```
GoedleAnalytics.track ("<event_name>");
```
*Example*
```
GoedleAnalytics.track ("start.game");
```

**Tracking a single event with an identifier**

```
GoedleAnalytics.track ("<event_name>", "<event_id>");
```
*Example*

```
GoedleAnalytics.track("reached.level", "level-2");
```

**Tracking a single event with an identifier and a value**
```
GoedleAnalytics.track("<event_name>","<event_id>","<event_value>");
```
*Example*
```
GoedleAnalytics.track("finish.level", "level-12", "65");
```

**Tracking the game state**

To track a continuous stream of events, the code of the Unity application needs additional preparation. This is the polling interval, which can be implemented with a counter (nextActionTime), that is increased after every action and the duration of one interval (period).

An example configuration with a 15 seconds interval would look like this:

```
// Game state config
private float nextActionTime = 0.0f; // 15.0f means a period of 15 seconds, this is complete sufficient to track a game state.
public float period = 15.0f;
```

Afterwards the tracking has to be periodically called. This can be achieved by using the update function of a Unity class.

```
void Update () {
// Your code ...
        if (Time.time > nextActionTime ) {
nextActionTime += period;
       	GoedleAnalytics.track("game.state","health", "100");
        GoedleAnalytics.track("game.state","mana","400");
        }
}
```
## Additional Functionalities

### Groups
If a one wants to track information about a class, school or a team, the SDK supports so called groups. A group consists of a type, for instance `class` and a group name `1A`.  Here is the complete call:

```
GoedleAnalytics.group ("<group_type>", "<group_name>");
```

*Example*
```
GoedleAnalytics.group ("school", "EA");
```
### Traits

To have the opportunity to further personalize the learning experience, additional information about a user can be helpful. Currently the goedle.io backend supports three different kinds of personal user information. Namely the last- / first name and the email address. It is important that these kind of information are only transmitted when user explicitly gave the permission to track these kind of information. The SDK only offers this function to track the information, but the developer should responsible use the functionality.
It is important that correct naming convention is used, otherwise the information can not be stored and processed.

**first_name**
```
GoedleAnalytics.trackTraits("first_name","<play_first_name>");
```
*Example*
```
GoedleAnalytics.trackTraits("first_name","Hans");
```

**last_name**
```
GoedleAnalytics.trackTraits("last_name","<player_last_name>");
```
*Example*
```
GoedleAnalytics.trackTraits("last_name", "Smith");
```

**email**
```
GoedleAnalytics.trackTraits("email", "<email_address>");
```
*Example*
```
GoedleAnalytics.trackTraits("email", "hans@gmail.com");
```

### Privacy
goedle.io is GDPR ready and respects the privacy. Therefore, the SDK hold functions to control the data which is transmitted to the goedle.io infrastructure.

#### Opt Out
To respect the privacy, it is necessary to add an opt-out function. This can be done via the interface or via a function call.  
GoedleAnalytics.disableTracking ();
This function disables the goedle.io SDK, and even if tracking points are set in the code, nothing is transmitted to the goedle.io backend or to Google Analytics.

#### User ID
If a user id is not available the goedle.io SDK creates automatically in the background a user id. But, the automatically created user ids are only session based. This is done, because of privacy reasons. Otherwise, the developer has to set explicit the user id in the code. For instance, if the user id is created on personal user information. To set a user id the function setUserId() can be called:

```
GoedleAnalytics.setUserId ("<unique_user_id>");
```
