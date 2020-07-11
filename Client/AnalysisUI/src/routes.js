import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

import ChartWrapper from './app/chartWrapper';
import TableWrapper from './app/tableWrapper';
import UnderConstruction from './app/underConstruction';
import E404 from './app/E404';

import {store} from './store';

const routes = [
	{
		path: '',
		redirect: {name: 'charts'}
	},
	{
		name: 'charts',
		path: '/charts',
		component: ChartWrapper,
	},
	{
        name: 'tables',
		path: '/tables',
		component: TableWrapper
	},
	{
		path: '*',
		component: E404
	}
];

export const router = new VueRouter({
	routes,
	mode: 'history'
});
