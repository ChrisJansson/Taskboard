module ``When adding a state``

open Xunit
open Swensen.Unquote

[<Fact>]
let ``A state is added to the board`` () =
    Given [ BoardCreated { Id = BoardId 1; Name = "Board"; } ] 
    |> When (CreateStateCommand { BoardId = BoardId 1; Name = "New state" })
    |> Then [ StateCreated { Id = StateId 0; Name = "New state" } ]

[<Fact>]
let ``Another state is added to the board`` () =
    Given [ 
        BoardCreated { Id = BoardId 1; Name = "Board"; }
        StateCreated { Id = StateId 0; Name = "State" } ] 
    |> When (CreateStateCommand { BoardId = BoardId 1; Name = "New state" }) 
    |> Then [ StateCreated { Id = StateId 1; Name = "New state" } ]
