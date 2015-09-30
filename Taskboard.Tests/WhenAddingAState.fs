module ``When adding a state``

open Xunit
open Swensen.Unquote

[<Fact>]
let ``A state is added to the board`` () =
    let command = CreateStateCommand { BoardId = BoardId 1; Name = "New state" }
    let events = Given [ BoardCreated { Id = BoardId 1; Name = "Board"; } ] |> When command
    let expectedEvents = [ StateCreated { Id = StateId 0; Name = "New state" } ]
    test<@ events = expectedEvents @>

[<Fact>]
let ``Another state is added to the board`` () =
    Given [ 
        BoardCreated { Id = BoardId 1; Name = "Board"; }
        StateCreated { Id = StateId 0; Name = "State" } ] 
    |> When (CreateStateCommand { BoardId = BoardId 1; Name = "New state" }) 
    |> Then [ StateCreated { Id = StateId 1; Name = "New state" } ]
