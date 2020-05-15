<template>
    <div>
        <!-- <h3 class="d-flex justify-content-center mt-1">{{ msg }}</h3> -->
        <div class="wrapper mx-3">
            <chart-wrapper 
                :country="'World'"
                :chartType="chartType"
                :fieldType="fieldType"
                :dataType="dataType">
            </chart-wrapper>
            <chart-wrapper 
                :country="country"
                :chartType="chartType"
                :fieldType="fieldType"
                :dataType="dataType">
            </chart-wrapper>
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
</template>

<script>
import {chartTypes, fieldTypes, dataTypes} from './app/common/constants.ts'
import ChartWrapper from './app/ChartWrapper'

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
            fieldTypeId: 0,
            fieldType: fieldTypes[0],
            dataTypeId: 0,
            dataType: dataTypes[0],
        }
    },
    computed: {
        nextChartType()
        {
            return this.getNextValue(chartTypes, this.chartTypeId + 1);
        },
        nextFieldType()
        {
            return this.getNextValue(fieldTypes, this.fieldTypeId + 1);
        },
        nextDataType()
        {
            return this.getNextValue(dataTypes, this.dataTypeId + 1);
        },
    },
    methods: {
        toggleChartType() {
            this.chartTypeId++;
            this.chartType = this.getNextValue(chartTypes, this.chartTypeId);
        },
        toggleFieldType() {
            this.fieldTypeId++;
            this.fieldType = this.getNextValue(fieldTypes, this.fieldTypeId);
        },
        toggleDataType() {
            this.dataTypeId++;
            this.dataType = this.getNextValue(dataTypes, this.dataTypeId);
        },
        getNextValue(arr, idx)
        {
            return arr[idx % arr.length];
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
</style>
