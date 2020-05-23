<template>
    <div>
        <section>
			<div class="container p-3">
				<div class="row">
					<div class="menu col col-sm-2">
						<ul class="list-group">
							<router-link v-for="(item, index) in menu"
										 :key="index"
										 :to="item.url"
										 tag="li"
										 class="list-group-item"
										 active-class="active"
							>
								<a>{{ item.text }}</a>
							</router-link>
						</ul>
					</div>
					<div class="main-zone col col-sm-10">
						<transition name="slide" mode="out-in">
							<router-view></router-view>
						</transition>
					</div>
				</div>
                <div class="row">
                    <div class="wrapper col my-3" v-if="isReady">
                        <button class="btn btn-primary m-2"
                            @click="toggleChartType">Switch to <b>{{nextChartType}}</b>
                        </button>
                        <button class="btn btn-primary m-2"
                            @click="toggleFieldType">Switch to <b>{{nextFieldType}}</b>
                        </button>
                        <button class="btn btn-primary m-2"
                            @click="toggleDataType">Switch to <b>{{nextDataType}}</b>
                        </button>
                    </div>
				</div>
			</div>
		</section>
    </div>
</template>

<script>
import { chartTypes, fieldTypes, dataTypes } from './app/common/constants'
import { getNextElement } from '../src/app/common/functions'
import ChartWrapper from './app/ChartWrapper'
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
        }
    },
    computed: {
        ...mapGetters('menu', {
            menu: 'items'
        }),
        nextChartType()
        {
            return getNextElement(chartTypes, this.chartType);
        },
        nextFieldType()
        {
            return getNextElement(fieldTypes, this.fieldType);
        },
        nextDataType()
        {
            return getNextElement(dataTypes, this.dataType);
        },
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
        toggleChartType() {
            this.chartType = getNextElement(chartTypes, this.chartType);
        },
        toggleFieldType() {
            this.fieldType = getNextElement(fieldTypes, this.fieldType);
        },
        toggleDataType() {
            this.dataType = getNextElement(dataTypes, this.dataType);
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
    #line-chart, #bar-chart{
        height: 350px;
    }
	.menu{
		border-right: 1px solid #ddd;
	}
	.list-group-item{
		transition: background 0.3s, color 0.3s;
	}
	.list-group-item a{
		text-decoration: none;
	}
	.list-group-item.active a{
		color: inherit;
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
