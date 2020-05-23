export default {
	namespaced: true,
	state: {
		items: [
			{
				url: '/charts',
				text: 'Charts'
			},
			{
				url: '/tables',
				text: 'Tables'
			},
		]
	},
	getters: {
		items(state){
			return state.items;
		}
	}
};