<template>
    <div>
        <!-- <h3 class="d-flex justify-content-center mt-1">{{ msg }}</h3> -->
        <div class="wrapper mx-3" v-if="isReady">
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
            chartTypeId: 0,
            chartType: null,
            fieldTypeId: 0,
            fieldType: null,
            dataTypeId: 0,
            dataType: null,
        }
    },
    computed: {
        ...mapGetters([
                'chartTypes',
                'fieldTypes',
                'dataTypes',
        ]),
        nextChartType()
        {
            return this.getNextValue(this.chartTypes, this.chartTypeId + 1);
        },
        nextFieldType()
        {
            return this.getNextValue(this.fieldTypes, this.fieldTypeId + 1);
        },
        nextDataType()
        {
            return this.getNextValue(this.dataTypes, this.dataTypeId + 1);
        },
    },
    mounted() {
            this.chartType = this.chartTypes[0];
            this.fieldType = this.fieldTypes[0];
            this.dataType = this.dataTypes[0];
            this.initializeComponent();
        },
    methods: {
        ...mapActions([
            'getAllCountries',
        ]),
        initializeComponent() {
            this.getAllCountries(this.dataTypes[0])
                .then(result => {
                    this.isReady = true;
                });
        },        
        toggleChartType() {
            this.chartTypeId++;
            this.chartType = this.getNextValue(this.chartTypes, this.chartTypeId);
        },
        toggleFieldType() {
            this.fieldTypeId++;
            this.fieldType = this.getNextValue(this.fieldTypes, this.fieldTypeId);
        },
        toggleDataType() {
            this.dataTypeId++;
            this.dataType = this.getNextValue(this.dataTypes, this.dataTypeId);
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
