var Taskboard = React.createClass({
	render: function() {
		var columnNodes = this.props.taskboard.columns.map(function (column) {
			return (
				<TaskboardColumn data={column} />
			);
		});
		return (
			<div className="taskboard">
				<div className="taskboardColumns">
					{columnNodes}
				</div>
				<TaskboardAddColumnForm />
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
	render: function() {
		return (
			<div>
				<form>
					<div>
						<label for="name">Name</label>
						<input id="name" type="text" />
					</div>
					<div>
						<input type="button" value="Add column" />
					</div>
				</form>
			</div>
		);
	}
});