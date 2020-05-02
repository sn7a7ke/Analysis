<template>
    <div>
        <!-- <h3 class="d-flex justify-content-center mt-1">{{ msg }}</h3> -->
        <div class="wrapper mx-3">
            <chart-wrapper 
                :country="'World'"
                :chartType="chartType"
                :dataType="dataType">
            </chart-wrapper>
            <chart-wrapper 
                :country="country"
                :chartType="chartType"
                :dataType="dataType">
            </chart-wrapper>
            <button class="btn btn-primary m-2"
                @click="toggleChartType">Toggle chart type
            </button>
            <button class="btn btn-primary m-2"
                @click="toggleDataType">Cases/Deaths/Recovered
            </button>
        </div>
    </div>
</template>

<script>
import {chartTypes, dataTypes} from './assets/Common/Constants.js'
import ChartWrapper from './assets/ChartWrapper'

export default {
    components: {
        ChartWrapper
    },
    data() {
        return {
            msg: 'Charts App',
            country: 'US',
            chartTypeId: 0,
            chartType: chartTypes[0],
            dataTypeId: 0,
            dataType: dataTypes[0]
        }
    },
    methods: {
        toggleChartType() {
            this.chartTypeId++;
            this.chartType = chartTypes[this.chartTypeId % chartTypes.length];
        },
        toggleDataType() {
            this.dataTypeId++;
            this.dataType = dataTypes[this.dataTypeId % dataTypes.length];
       },
    },
    computed: {
    },
    beforeMount(){
    }
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
</style>
