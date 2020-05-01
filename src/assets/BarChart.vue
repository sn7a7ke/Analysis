<script>
import { Bar, mixins  } from 'vue-chartjs'

export default {
    extends: Bar,
    mixins: [mixins.reactiveProp],
    props: ['options'], //, 'refresh'],
    mounted () {
        this.renderLineChart();
    },
    methods: {
        renderLineChart() {
            this.renderChart(this.chartData, this.options);
        }
    },
    // watch: {
    //     refresh: function() {
    //         this._chart.destroy();
    //         this.renderLineChart();
    //     }
    // }  
}
</script>