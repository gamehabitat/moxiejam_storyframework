---
uid: persistency_in_scripts
title: Persistency in scripts
---
# Persistency in scripts

Persistency is useful to share values between scenes in a game. It could be things such as wether a button was pressed or if the pizza at the restaurant was paid for.

Any object needing persistency always need to also have `PersistentObject` (see <xref:component_persistent_object>)  component attached. This component provide the persistency with a identifier so this game object know that I am me and any data associated with this identifier is for me. You can see PersistentObject and the identifier as your personal identification number or your name.  Going by this analogy, the persistent values are things associated with you:

* Wether I've been at this resturant before
* If I got money for the pizza
* If I left a nice review to the pizza baker.

Let's now say you want to store how many times you've been in a room (scene) in the game.. there's no pre-made script for doing this so we will make a new script for this.
The script will use a `GameStateValue<int>` to store the room counting. It uses the identifier `EnterRoomCounter_EnteredThisRoomCounter` to identify this game state value.

On Start of the script it tells the counter to run the method `OnOnValueModified` whenever the counter value is updated.

When a scene is loaded the counter value will be set to a default value (in the case of a number it will be set to 0). The nice thing with the persistency system is that the real counter value is actually stored away and can be used to restore the counter value. When the scene is loaded the method `LoadPersistentData` is called and can be used to restore the counter value from the persistency system as can be seen in the example code.

```c#
using UnityEngine;
using StoryFramework;
using UnityEngine.Events;

// RequireComponent makes sure that a PersistentObject component is also created when this component is created.
[RequireComponent(typeof(PersistentObject))]
public class EnterRoomCounter : MonoBehaviour, IPersistentComponent
{
    // This is a identifier for the counter value that will be associated with the identifier of PersistentObject. 
    public const string EnteredThisRoomCounterStateId = "EnterRoomCounter_EnteredThisRoomCounter";

    // An event that is called when ever the room counter is updated. It will provide the current counter value.
    [SerializeField]
    private UnityEvent<string> onRoomCounterUpdated;

    // How many times we entered the room.
    [System.NonSerialized]
    public GameStateValue<int> EnteredThisRoomCounter = new GameStateValue<int>();

    public void Start()
    {
        // Make it so OnOnValueModified is executed when ever the counter gets a new value.
        EnteredThisRoomCounter.OnValueModified += OnOnValueModified;
        
        // Increase the "entered this room counter" when the script is activated.
        EnteredThisRoomCounter.Value++;
    }

    void OnOnValueModified()
    {
        // Execute the room counter updated event.
        onRoomCounterUpdated?.Invoke(EnteredThisRoomCounter.Value.ToString());
    }

    // Called right before the scene starts and can be used to restore any previously saved data.  
    public void LoadPersistentData(GameSaveData saveData)
    {
        // Try and restore any previously save data and set it to EnteredThisRoomCounter,
        // if this is the first time the room is entered, the counter will be set to 0. 
        EnteredThisRoomCounter = saveData.GetState(this, EnteredThisRoomCounterStateId, 0);
    }
}
```


## Related components

* <xref:component_persistent_object>
