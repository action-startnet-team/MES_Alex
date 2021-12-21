

jQuery(function ($) {

    let $lineItems = $('#pome_dashboard').find('.line-item')
    console.log($lineItems);
    let pTkCode_list = []
    $lineItems.each(function () {
        pTkCode_list.push($(this).attr('id'))
    })

    function updateDashBoard(interval_ms) {
        $.ajax({
            method: 'post',
            url: '/DSB170A/Get_All_LineData',
            data: {
                pTkCode_List: pTkCode_list
            }
        }).done(function (json) {
            $lineItems.each(function (i) {
                let TkCode = $(this).attr('id')
                let lineItem_data = json[TkCode]
                Set_LineItem_Text($(this), lineItem_data)

                // 暫時這樣做
                let heading1_data = lineItem_data.heading1_items
                let alarm = lineItem_data.alarmMsg
                $(this).closest('.container').find('.heading1-item').each(function (i) {
                    let itemData = heading1_data[i]
                    if (itemData) {
                        $(this).find('.heading1-item-label').text(itemData.label)
                        $(this).find('.heading1-item-value').text(itemData.value)
                        
                    }
                })
                $(this).closest('.container').find('.breathlight').each(function (i) {
                    let itemData = alarm[i]
                    
                    if (itemData!= '') {
                        $(this).find('.breathlight-item-alarm').text(itemData.value)
                    } 
                   
                })
            })

        }).always(function () {
            setTimeout(function () {
                updateDashBoard(interval_ms)
            }, interval_ms)

        })
    }

    updateDashBoard(2000)  // 開始更新，傳入更新間隔(ms)


    function Set_LineItem_Text(el, data) {

        let $el = $(el)

        let $heading2_items = $el.find('.heading2-item')
        let $cell_items = $el.find('.cell-item')
        let $oee_items = $el.find('.oee-item')

        let heading2_data = data.heading2_items
        let cell_data = data.cell_items
        let oee_data = data.oee_items

        let gauge_val = data.gauge_params.value


        $heading2_items.each(function (i) {
            let item_data = heading2_data[i]
            if (item_data) {
                $(this).attr('data-name', item_data.name)
                $(this).find('.heading2-item-label').text(item_data.label)
                $(this).find('.heading2-item-value').text(item_data.value)
            }
        })

        $cell_items.each(function (i) {
            let item_data = cell_data[i]
            if (item_data) {
                $(this).attr('data-name', item_data.name)
                $(this).find('.cell-item-label').text(item_data.label)
                $(this).find('.cell-item-value').text(item_data.value)
            }
        
        })

        $oee_items.each(function (i) {
            let item_data = oee_data[i]
            if (item_data) {
                $(this).attr('data-name', item_data.name)
                $(this).find('.oee-item-label').text(item_data.label)
                $(this).find('.oee-item-value').text(item_data.value)
            }
        })


        // chart 
        Set_Chart_Options(data.gauge_params)
        let chartElement =  $el.find('.chart-gauge')[0] //document.getElementById("chart-gauge-A01")
        var myChart = echarts.init(chartElement);
        myChart.clear()
        myChart.setOption(dashboard_option);
    }



    /*  echart gauge 相關script */
    var db_options = {
	    "data": [{
		    "value": null,
		    "name": ""
	    }],
	    "color": [],
	    "title": {
		    show: true,
		    offsetCenter: [0, '95%'],
		    color: '',
		    fontSize: 18,
		    backgroundColor: "",
		    borderRadius: 5,
		    padding: 5
	    }
    };

    var dashboard_option = { 
	    series: [{
			    name: "",
			    type: "gauge",
			    center: ["50%", "47%"],
			    radius: "85%",
			    min: 0,
			    max: 100,
			    data: db_options.data,
			    axisLine: {
				    show: true,
				    lineStyle: { 
					    color: db_options.color,
					    shadowColor: "#ccc",
					    shadowBlur: 10,
					    width: 5
				    }
			    },
			    splitLine: {
				    show: false
			    },
			    axisTick: {
				    show: false
			    },
			    axisLabel: {
				    show: false
			    },
			    pointer: { 
				    length: '60%',
				    color: "#4A90E2"
			    },
			    title: db_options.title,
		    },

	    ]
    };

    function Set_Chart_Options(params) {
        let value = params.value
        db_options.data[0].value = value;
        db_options.color.splice(0, db_options.color.length);
        let options = params.options
        let a01 = options[0].baseLine
        let a02 = options[1].baseLine
        let a03 = options[2].baseLine
        if (0 <= value && value < a01) {
	        db_options.data[0].name = options[0].name;
		    db_options.color.push([value / 100, '#ff624b']);
		    db_options.title.color = '#ff624b';
		    db_options.title.backgroundColor = "rgba(255, 98, 75, 0.4)";
        } else if (a01 <= value && value < a02) {
	        db_options.data[0].name = options[1].name;
		    db_options.color.push([value / 100, '#ffca36']);
		    db_options.title.color = '#ffca36';
		    db_options.title.backgroundColor = "rgba(255, 202, 54, 0.4)";
        } else if (a02 <= value && value <= a03) {
	        db_options.data[0].name = options[2].name;
		    db_options.color.push([value / 100, '#63F371']);
		    db_options.title.color = '#63F371';
		    db_options.title.backgroundColor = "rgba(99, 243, 113, 0.4)";

	    } else {}
	    db_options.color.push([1, '#5a5a5a']);
    }

    //function db_init(value) {     
	//    setGaugeValue(value);
	//    var option_dashboard = dashboard_option;
	//    return option_dashboard;
    //}

    //option = db_init(73);

    //var myChart2 = echarts.init(document.getElementById("chart-gauge-A02"));
    //myChart2.setOption(option);

    //var myChart3 = echarts.init(document.getElementById("chart3"));
    //myChart3.setOption(option);

    //var myChart4 = echarts.init(document.getElementById("chart4"));
    //myChart4.setOption(option);
})