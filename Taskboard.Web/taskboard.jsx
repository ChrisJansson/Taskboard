var Taskboard = React.createClass({
	getInitialState: function() {
		return {data: this.props.taskboard};
	},
	handleColumnCreateSubmit: function(column) {
		var newColumn = {
			name: column.name,
			cards: []
		};
		
		var board = this.state.data;
		var newBoard = React.addons.update(board, {
			columns: { $push: [newColumn] }
		});
		
		this.setState({data: newBoard});
		return;
	},
	render: function() {
		var columnNodes = this.state.data.columns.map(function (column) {
			return (
				<TaskboardColumn data={column} />
			);
		});
		return (
			<div className="taskboard">
				<div className="taskboardColumns">
					{columnNodes}
				</div>
				<TaskboardAddColumnForm onColumnCreateSubmit={this.handleColumnCreateSubmit}/>
			</div>
		);
	}
});

var TaskboardColumn = React.createClass({
	render: function() {
		var cardNodes = this.props.data.cards.map(function (card) {
			return (
				<TaskboardCard data={card} />
			);
		});
		return (
			<div className="taskboardColumn panel panel-default">
				<div className="panel-heading taskboardColumnHeader">
					{this.props.data.name}
				</div>
				<div>
					{cardNodes}
				</div>
			</div>
		);
	}
});

var TaskboardCard = React.createClass({
	render: function() {
		return (
			<div className="taskboardCard panel panel-primary" draggable="true">
				<div className="taskboardColumnCardHeader panel-heading">
					{this.props.data.name}
				</div>
				{this.props.data.text}
			</div>
		);
	}
});

var TaskboardAddColumnForm = React.createClass({
	handleSubmit: function(e) {
		e.preventDefault();
		var name = React.findDOMNode(this.refs.name).value.trim();
		this.props.onColumnCreateSubmit({name: name});
		return;
	},
	render: function() {
		return (
			<div>
				<form onSubmit={this.handleSubmit}>
					<div>
						<label for="name">Name</label>
						<input id="name" ref="name" type="text" />
					</div>
					<div>
						<input type="submit" value="Add column" />
					</div>
				</form>
			</div>
		);
	}
});