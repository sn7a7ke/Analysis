<template>
    <v-app>
        <v-navigation-drawer app>
            <!-- -->
        </v-navigation-drawer>
        <v-card class="overflow-hidden">
        <v-app-bar
                absolute
                color="#6A76AB"
                dark                
                dense
                src="https://picsum.photos/1920/1080?random"
                fade-img-on-scroll
                scroll-target="#scrolling-techniques-4"
                >
            <template v-slot:img="{ props }">
                <v-img v-bind="props" gradient="to top right, rgba(100,115,201,.7), rgba(25,32,72,.7)" ></v-img>
            </template>
            <v-app-bar-nav-icon></v-app-bar-nav-icon>
            <v-toolbar-title>Analysis</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon>
                <v-icon>mdi-magnify</v-icon>
            </v-btn>
            <v-btn icon>
                <v-icon>mdi-heart</v-icon>
            </v-btn>
            <v-menu bottom left >
                <template v-slot:activator="{ on }">
                    <v-btn icon color="grey" v-on="on">
                        <v-icon>mdi-dots-vertical</v-icon>
                    </v-btn>
                </template>
                <v-list>
                    <v-list-item v-for="(item, i) in menuItems" :key="i">
                        <v-list-item-title>{{ item.title }}</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
            <template v-slot:extension>
                <v-tabs align-with-title>
                    <router-link v-for="(item, index) in menu"
                            :key="index"
                            :to="item.url"
                            tag="v-tab"
                            >
                        {{ item.text }}
                    </router-link>
                </v-tabs>
            </template>
        </v-app-bar>
        <v-sheet
                id="scrolling-techniques-4"
                class="overflow-y-auto"
                max-height="800"
                >
            <v-container fluid class="a-container">
                <transition name="slide" mode="out-in">
                    <router-view v-if="isReady"></router-view>
                </transition>
            </v-container>
        </v-sheet>
        </v-card>
        <v-footer app>
            <!-- -->
        </v-footer>
    </v-app>
</template>

<script>
import { chartTypes, fieldTypes, dataTypes } from './app/common/constants'
import { getNextElement } from '../src/app/common/functions'
import ChartWrapper from './app/chartWrapper'
import { mapGetters } from 'vuex';
import { mapActions } from 'vuex';

export default {
    components: {
        ChartWrapper
    },
    data() {
        return {
            msg: 'Charts App',
            isReady: false,
            country: 'US',
            chartType: chartTypes[0],
            fieldType: fieldTypes[0],
            dataType: dataTypes[0],
            menuItems: [
                { title: 'Some text' },
                { title: 'Some text2' },
            ],
        }
    },
    computed: {
        ...mapGetters('menu', {
            menu: 'items'
        }),
    },
    mounted() {
            this.initializeComponent();
        },
    methods: {
        ...mapActions('covid', {
            getAllCountries: 'getAllCountries',
        }),
        initializeComponent() {
            this.getAllCountries(dataTypes[0])
                .then(result => {
                    this.isReady = true;
                });
        },        
    },
}
</script>

<style scoped>
    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }
    .a-container {
        margin-top: 100px;
        height: 1000px;
        min-width: 600px;
        max-width: 1024px;
    }
    
	.slide-enter{
		
	}
	.slide-enter-active{
		animation: slideIn 0.5s;
	}
	.slide-enter-to{
		
	}
	.slide-leave{
		
	}
	.slide-leave-active{
		animation: slideOut 0.5s;
	}
	.slide-leave-to{
		
	}
	@keyframes slideIn{
		from{transform: rotateY(90deg);}
		to{transform: rotateY(0deg);}
	}
	@keyframes slideOut{
		from{transform: rotateY(0deg);}
		to{transform: rotateY(90deg);}
	}
</style>
