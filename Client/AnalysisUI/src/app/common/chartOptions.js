export const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
        xAxes: [{
            ticks: {
                maxRotation: 90,
                minRotation: 90
            }
        }],
        yAxes: [{
            ticks: {
                suggestedMin: 0,
            }
        }]
    },
    legend: {
        display: false,
    }
}