[<AutoOpen>]
module Domain


type CardId = CardId of int

type Card = 
    {
        Id : CardId
    }

type ColumnId = ColumnId of int

type Column = 
    {
        Id : ColumnId
        Name : string
        Cards : Card list
    }
    
type BoardId = BoardId of int

type Board =
    {
        Id : BoardId
        Name : string
        Columns : Column list
        Created : bool
    }
    with 
    static member initial = { Id = BoardId 0; Name = "None"; Columns = []; Created = false }

    
type Command =
    | CreateBoardCommand of CreateBoardCommand

and CreateBoardCommand = 
    {
        Id : BoardId
        Name : string
    }

type Event =
    | BoardCreated of BoardCreated

and BoardCreated =
    {
        Id : BoardId
        Name : string
    }

let createBoard (command:CreateBoardCommand) state =
    if not state.Created then
        [ { BoardCreated.Id = command.Id; Name = command.Name } ]
    else failwith "The board is already created"

let handle command state  =
    match command with
    | CreateBoardCommand createBoardCommand -> createBoard createBoardCommand state

let evolve event state =
    match event with
    | BoardCreated bc ->
        { Id = bc.Id; Name = bc.Name; Columns = []; Created = true }
        
