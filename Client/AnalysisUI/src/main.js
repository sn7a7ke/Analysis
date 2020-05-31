import Vue from 'vue'
import vuetify from './plugins/vuetify'
import App from './App.vue'
import {store} from './store/index'
import {router} from './routes.js';

new Vue({
  el: '#app',
  vuetify,
  store,
  router,
  render: h => h(App)
})
