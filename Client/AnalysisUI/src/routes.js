import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

import chartWrapper from './app/chartWrapper';
import tableWrapper from './app/tableWrapper';
import underConstruction from './app/underConstruction';
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
		component: chartWrapper,
	},
	{
        name: 'tables',
		path: '/tables',
		component: underConstruction
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
