module ``When creating a board``

open Xunit
open Swensen.Unquote
open System

[<Fact>]
let ``Board should be created for initial state``() =
    let state = Given []
    let command = CreateBoardCommand { Id = BoardId 1; Name = "New board name"; }
    let producedEvents = handle command state
    let expectedEvents = [ BoardCreated { Id = BoardId 1; Name = "New board name"; } ]
    test <@ expectedEvents = producedEvents @>

[<Fact>]
let ``Board can not be recreated``() =
    let state = Given [ BoardCreated { Id = BoardId 1; Name = "New board name"; } ]
    let command = CreateBoardCommand { Id = BoardId 1; Name = "Create again"; }
    let act () = handle command state |> ignore
    Assert.Throws<System.Exception>(Action act)
