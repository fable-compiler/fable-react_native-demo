module Home

open Fable.ReactNative
open Fable.ReactNative.Props
open Elmish
open Fable.Remoting.Client
open Shared

// Model

type Model = { StatusText : string  }

type Msg =
| GetDemoData
| NewDemoData of int
| BeginWatch
| Error of exn


let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

// Update
let update (msg:Msg) model : Model*Cmd<Msg> =
    match msg with
    | GetDemoData ->
        { model with StatusText = "Syncing..." },
        Cmd.OfPromise.either Database.createDemoData () NewDemoData Error

    | NewDemoData count ->
        { model with StatusText = sprintf "Locations: %d" count }, Cmd.none

    | BeginWatch ->
        model, Cmd.none // Handled one level above

    | Error e ->
        { model with StatusText = string e.Message }, Cmd.none


let init () = { StatusText = "" }, Cmd.ofMsg GetDemoData

// View

let view (model:Model) (dispatch: Msg -> unit) =
      printfn "%A" (localImage "../../images/raven.jpg")
      let logo =
          image
              [ Source (localImage "../../images/raven.jpg")
                ImageProperties.Style [
                  FlexStyle.AlignSelf Alignment.Center
                  FlexStyle.FlexDirection FlexDirection.Column
                ]
              ]
 
      view [ Styles.sceneBackground ]
        [ text [ Styles.titleText ] "Client"
          logo
          Styles.button "Begin watch" (fun () -> dispatch BeginWatch)
          Styles.whitespace
          Styles.whitespace
          text [ Styles.smallText ] model.StatusText  ]
