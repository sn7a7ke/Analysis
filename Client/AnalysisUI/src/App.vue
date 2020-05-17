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
            chartType: null,
            fieldType: null,
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
            return getNextElement(this.chartTypes, this.chartType);
        },
        nextFieldType()
        {
            return getNextElement(this.fieldTypes, this.fieldType);
        },
        nextDataType()
        {
            return getNextElement(this.dataTypes, this.dataType);
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
            this.chartType = getNextElement(this.chartTypes, this.chartType);
        },
        toggleFieldType() {
            this.fieldType = getNextElement(this.fieldTypes, this.fieldType);
        },
        toggleDataType() {
            this.dataType = getNextElement(this.dataTypes, this.dataType);
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
