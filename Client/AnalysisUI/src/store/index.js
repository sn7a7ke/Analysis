import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

import menu from './modules/menu';
import covid from './modules/covid';

export const store = new Vuex.Store({
    modules: {
        menu,
        covid,
	},
    strict: process.env.NODE_ENV !== 'production',
});
