using UnityEngine;

public class Ability
{
    public readonly AbilityData data;

    public Ability(AbilityData data)
    {
        this.data = data;
    }

    public AbilityCommand CreateCommand()
    {
        return new AbilityCommand(data);
    }
}



public class Model
{
    // what's the model: data, state, business logic
    // model is the only thing that can talk to the database
    // model do this: update data, notify presenter
    ///Model: Sadece değerleri tutan bir veri kabıdır. Oyun mantığını gerçekleştirmez veya hesaplamalar yapmaz.
}
public class View
{
    // what's the view: UI, input, output
    // view is the only thing that can talk to the user
    // view do this: listen to user input, display output
    ///View: Kullanıcıya veri gösteren ve kullanıcıdan veri alan bir arayüzdür. Oyun mantığını gerçekleştirmez veya hesaplamalar yapmaz.
    // Examples : UI, 3D model, sound, keyboard, mouse, touch, camera, screen
}
public class Presenter
{
    //what's the presenter: mediator, controller, adapter
    // presenter is the only thing that can talk to the model
    // presenter do this: listen to view, update model, update view
    ///Presenter: Model ve View arasında köprü görevi görür. Kullanıcıdan gelen veriyi alır ve modeli günceller. Modeldeki değişiklikleri dinler ve View'i günceller.
    //Presenter'da hangi methodlar olmalı: Start, Update, FixedUpdate, LateUpdate, OnGUI, OnDestroy, OnApplicationQuit, OnApplicationPause, OnApplicationFocus, OnApplicationUpdate, OnApplicationFixedUpdate, OnApplicationLateUpdate, OnApplicationOnGUI, OnApplicationOnDestroy
    // Examples : Controller, Mediator, Adapter
}

#region MVP Example
// Model
public class GameModel
{
    public Vector3 Position { get; set; }
}

// View
public class GameView : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

// Presenter
public class GamePresenter
{
    private GameModel _model;
    private GameView _view;

    public GamePresenter(GameView view)
    {
        _model = new GameModel();
        _view = view;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        _model.Position = newPosition;
        _view.SetPosition(_model.Position);
    }
}

public class GameController : MonoBehaviour
{
    private GamePresenter _presenter;

    void Start()
    {
        // GameView referansını alın
        GameView view = GetComponent<GameView>();

        // GamePresenter'ı başlatın
        _presenter = new GamePresenter(view);
    }

    void Update()
    {
        // Pozisyonu güncelleyin
        Vector3 newPosition = new Vector3(1, 0, 0);
        _presenter.UpdatePosition(newPosition);
    }
}
#endregion
