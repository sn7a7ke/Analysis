import Vue from 'vue'
import App from './App.vue'
import {store} from './store/index'
import {router} from './routes.js';

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
