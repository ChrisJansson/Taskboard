module ``When evolving state``

open Xunit
open Swensen.Unquote

let runFullEventStream events =
    events |> List.fold (fun s e -> evolve e s) Board.initial

[<Fact>]
let ``Evolves column moving down`` () =
    let board = 
        [ 
            BoardCreated { Id = BoardId 0; Name = "New board" } 
            StateCreated { Id = StateId 0; Name = "State 0" } 
            StateCreated { Id = StateId 1; Name = "State 1" } 
            StateCreated { Id = StateId 2; Name = "State 2" } 
            ColumnCreated { Id = ColumnId 0; State = StateId 0; Name = "Column 0" }
            ColumnCreated { Id = ColumnId 1; State = StateId 1; Name = "Column 1" }
            ColumnCreated { Id = ColumnId 2; State = StateId 2; Name = "Column 2" }
            ColumnMoved { Id = ColumnId 2; Position = 1; }
        ] 
        |> runFullEventStream

    let expected = {
            Id = BoardId 0
            Name = "New board"
            States = [
                        { Id = StateId 0; Name = "State 0" }
                        { Id = StateId 1; Name = "State 1" }
                        { Id = StateId 2; Name = "State 2" }
                ]
            Columns = [
                        { Id = ColumnId 0; Name = "Column 0"; }
                        { Id = ColumnId 2; Name = "Column 2"; }
                        { Id = ColumnId 1; Name = "Column 1"; }
                ]
            Created = true
        }
    test<@ board = expected @>

[<Fact>]
let ``Evolves column moving up`` () =
    let board = 
        [ 
            BoardCreated { Id = BoardId 0; Name = "New board" } 
            StateCreated { Id = StateId 0; Name = "State 0" } 
            StateCreated { Id = StateId 1; Name = "State 1" } 
            StateCreated { Id = StateId 2; Name = "State 2" } 
            StateCreated { Id = StateId 3; Name = "State 3" } 
            StateCreated { Id = StateId 4; Name = "State 4" } 
            ColumnCreated { Id = ColumnId 0; State = StateId 0; Name = "Column 0" }
            ColumnCreated { Id = ColumnId 1; State = StateId 1; Name = "Column 1" }
            ColumnCreated { Id = ColumnId 2; State = StateId 2; Name = "Column 2" }
            ColumnCreated { Id = ColumnId 3; State = StateId 3; Name = "Column 3" }
            ColumnCreated { Id = ColumnId 4; State = StateId 4; Name = "Column 4" }
            ColumnMoved { Id = ColumnId 1; Position = 3; }
        ] 
        |> runFullEventStream

    let expected = {
            Id = BoardId 0
            Name = "New board"
            States = [
                        { Id = StateId 0; Name = "State 0" }
                        { Id = StateId 1; Name = "State 1" }
                        { Id = StateId 2; Name = "State 2" }
                        { Id = StateId 3; Name = "State 3" }
                        { Id = StateId 4; Name = "State 4" }
                ]
            Columns = [
                        { Id = ColumnId 0; Name = "Column 0"; }
                        { Id = ColumnId 2; Name = "Column 2"; }
                        { Id = ColumnId 3; Name = "Column 3"; }
                        { Id = ColumnId 1; Name = "Column 1"; }
                        { Id = ColumnId 4; Name = "Column 4"; }
                ]
            Created = true
        }
    test<@ board = expected @>
