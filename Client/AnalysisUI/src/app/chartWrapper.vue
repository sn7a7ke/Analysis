<template>
    <div class="chart-wrapper border border-info rounded" v-if="isReady">
        <div class="d-flex align-items-center m-1">
            <v-select class="country-dropdown p-0" 
                :options="countries"
                :clearable="false"
                v-model="verifiedCountry" 
                @input="countryChanged">
            </v-select>
            <div class="p-1" height="28" :class="chartTypeClass"
                    @mouseover="chartTypeMouseover()"
                    @click="chartTypeClick()">
                <img src="../../css/chart-bar.svg" height="28"
                        v-if="$_chartType=='bar'"/>
                <img src="../../css/chart-line.svg" height="28"
                        v-else/>
            </div>
            <div class="col d-flex align-items-center p-0">
                <div class="summary pointer p-1" :class="dataTypeClass"
                        @mouseover="dataTypeMouseover()"
                        @click="dataTypeClick()">
                    {{$_dataType.toUpperCase()}}
                </div>
                <div class="summary pointer p-1" :class="isFieldType(index)"
                        v-for="(value, name, index) in summary" :key="index"
                        @click="fieldTypeClick(index)">
                    {{name | capitalizeFirstLetter}}: <b>{{value | numeric}}</b>
                </div>
            </div>
        </div>
        <div class="m-1">
            <line-chart
                v-if="$_chartType=='line'"
                :chart-data="datacollection"
                :options="options">
            </line-chart>
            <bar-chart
                v-if="$_chartType=='bar'"
                :chart-data="datacollection"
                :options="options">
            </bar-chart>
        </div>
    </div>
</template>

<script>
import vSelect from 'vue-select'
import { chartTypes, fieldTypes, dataTypes } from './common/constants'
import { getNextElement, getKey } from './common/functions'
import { chartOptions } from './common/chartOptions'
import LineChart from './lineChart'
import BarChart from './barChart'
import { mapGetters } from 'vuex';
import { mapActions } from 'vuex';


export default {
    components: {
        vSelect,
        LineChart,
        BarChart
    },
    filters: {
        numeric(value) {
            return new Intl.NumberFormat().format(value);
        },
        capitalizeFirstLetter(value) {
            return value[0].toUpperCase() + value.substring(1).toLowerCase();
        },
    },
    props: {
        country: {
            type: String,
            default: this.defaultCountry,
        },
        chartType: {
            type: String,
            default: chartTypes[0],
            validator(value) {
                return chartTypes.includes(value);
            }
        },
        fieldType: {
            type: String,
            default: fieldTypes[0],
            validator(value) {
                return fieldTypes.includes(value);
            }
        },
        dataType: {
            type: String,
            default: dataTypes[0],
            validator(value) {
                return dataTypes.includes(value);
            }
        },
    },
    data(){
        return {
            datacollection: null,
            options: chartOptions,
            isReady: false,
            verifiedCountry: '',
            $_chartType: '',
            $_fieldType: '',
            $_dataType: '',
            defaultCountry: 'World',
            chartTypeClassToggler: true,
            dataTypeClassToggler: true,
        }
    },
    computed: {
        ...mapGetters('covid', {
                countries: 'countries',
                summaryByKey: 'summaryByKey',
                countryDataByKey: 'countryDataByKey',
        }),
        summary() {
            let key = getKey(this.verifiedCountry, this.$_dataType);
            let res = { ...this.summaryByKey(key)};
            delete res.date;
            return res;
        },
        chartTypeClass() {
            return {'animate__animated': this.chartTypeClassToggler, 'animate__heartBeat': this.chartTypeClassToggler};
        },
        dataTypeClass() {
            return {'animate__animated': this.dataTypeClassToggler, 'animate__heartBeat': this.dataTypeClassToggler};
        },
    },
    watch: {
        chartType() {
            this.$_chartType = this.chartType
            this.refresh();
        },
        fieldType() {
            this.$_fieldType = this.fieldType;
            this.fillData();
        },
        dataType() {
            this.$_dataType = this.dataType
            this.refresh();
        },
    },
    mounted() {
        this.$_chartType = this.chartType;
        this.$_fieldType = this.fieldType;
        this.$_dataType = this.dataType;
        this.initializeComponent();
    },
    beforeUpdate() {
    },
    methods: {
        ...mapActions('covid', {
            getSummary: 'getSummary',
            getCountryData: 'getCountryData',
        }),        
        initializeComponent() {
            this.verifyCountry();
            return this.refresh()
                .then(result => {
                    this.isReady = true;
                });
        },
        verifyCountry() {
            this.verifiedCountry = (this.countries.includes(this.country))
                    ? this.country
                    : this.defaultCountry;
        },
        refresh() {
            let payLoad = {
                dataType: this.$_dataType, 
                country: this.verifiedCountry
            };
            return Promise.all([
                    this.getSummary(payLoad),
                    this.getCountryData(payLoad)
                ])
                .then(result => {
                    this.fillData();
                });
        },
        fillData() {
            this.datacollection = {
                labels: this.countryData().map(d => d.date),
                datasets: [{
                    backgroundColor: '#888899',
                    data: this.countryData().map(d => d[this.$_fieldType])
                }]
            }
        },
        countryData() {
            let key = getKey(this.verifiedCountry, this.$_dataType);
            return this.countryDataByKey(key);
        },
        countryChanged() {
            this.refresh();
        },
        chartTypeClick() {
            this.$_chartType = getNextElement(chartTypes, this.$_chartType);
            this.chartTypeClassToggler = true;
            setTimeout(() => this.chartTypeClassToggler = false, 800);
            this.refresh();
        },
        fieldTypeClick(i) {
            this.$_fieldType = fieldTypes[i];
            this.fillData();
        },
        dataTypeClick() {
            this.$_dataType = getNextElement(dataTypes, this.$_dataType);
            this.dataTypeClassToggler = true;
            setTimeout(() => this.dataTypeClassToggler = false, 800);
            this.refresh();
        },
        isFieldType(i) {
            let expr = fieldTypes[i] == this.$_fieldType;
            return {'active': expr, 'animate__animated': expr, 'animate__heartBeat': expr};
        },
        chartTypeMouseover() {
            this.chartTypeClassToggler = false;
        },
        dataTypeMouseover() {
            this.dataTypeClassToggler = false;
        },
    },
}
</script>

<style scoped>
@import '../../node_modules/vue-select/dist/vue-select.css';
.chart-wrapper {
    text-align: left;
}
.active {
    background-color: #ddddee;
    border-radius: 5px;
}
.pointer:hover {
    cursor: pointer;
}
.summary {
    text-align: center;
}
.country-dropdown {
    min-width: 250px;    
}
</style>