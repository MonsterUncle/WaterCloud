function infographicTheme() {

    var theme = {
        // Ĭ��ɫ��
        color: [
            '#C1232B','#B5C334','#FCCE10','#E87C25','#27727B',
            '#FE8463','#9BCA63','#FAD860','#F3A43B','#60C0DD',
            '#D7504B','#C6E579','#F4E001','#F0805A','#26C0C0'
        ],

       
        // �������ſ�����
        dataZoom: {
            dataBackgroundColor: 'rgba(181,195,52,0.3)',            // ���ݱ�����ɫ
            fillerColor: 'rgba(181,195,52,0.2)',   // �����ɫ
            handleColor: '#27727B'    // �ֱ���ɫ
        },

        // ����
        grid: {
            borderWidth:0
        },

        // ��Ŀ��
        categoryAxis: {
            axisLine: {            // ��������
                lineStyle: {       // ����lineStyle����������ʽ
                    color: '#27727B'
                }
            },
            splitLine: {           // �ָ���
                show: false
            }
        },

        // ��ֵ��������Ĭ�ϲ���
        valueAxis: {
            axisLine: {            // ��������
                show: false
            },
            splitArea : {
                show: false
            },
            splitLine: {           // �ָ���
                lineStyle: {       // ����lineStyle�����lineStyle������������ʽ
                    color: ['#ccc'],
                    type: 'dashed'
                }
            }
        },

        polar : {
            axisLine: {            // ��������
                lineStyle: {       // ����lineStyle����������ʽ
                    color: '#ddd'
                }
            },
            splitArea : {
                show : true,
                areaStyle : {
                    color: ['rgba(250,250,250,0.2)','rgba(200,200,200,0.2)']
                }
            },
            splitLine : {
                lineStyle : {
                    color : '#ddd'
                }
            }
        },

        timeline : {
            lineStyle : {
                color : '#27727B'
            },
            controlStyle : {
                normal : { color : '#27727B'},
                emphasis : { color : '#27727B'}
            },
            symbol : 'emptyCircle'
        },

        // ����ͼĬ�ϲ���
        line: {
            itemStyle: {
                normal: {
                    borderWidth:2,
                    borderColor:'#fff',
                    lineStyle: {
                        width: 3
                    }
                },
                emphasis: {
                    borderWidth:0
                }
            },
            symbol: 'circle',  // �յ�ͼ������
        },

        // K��ͼĬ�ϲ���
        k: {
            itemStyle: {
                normal: {
                    color: '#C1232B',       // ���������ɫ
                    color0: '#B5C334',      // ���������ɫ
                    lineStyle: {
                        width: 1,
                        color: '#C1232B',   // ���߱߿���ɫ
                        color0: '#B5C334'   // ���߱߿���ɫ
                    }
                }
            }
        },

        // ɢ��ͼĬ�ϲ���
        scatter: {
            itemStyle: {
                normal: {
                    borderWidth:1,
                    borderColor:'rgba(200,200,200,0.5)'
                },
                emphasis: {
                    borderWidth:0
                }
            },
            symbol: 'star4',    // ͼ������
        },

        // �״�ͼĬ�ϲ���
        radar : {
            symbol: 'emptyCircle',    // ͼ������
            //symbol: null,         // �յ�ͼ������
            //symbolRotate : null,  // ͼ����ת����
        },

        map: {
            itemStyle: {
                normal: {
                    areaStyle: {
                        color: '#ddd'
                    },
                    label: {
                        textStyle: {
                            color: '#C1232B'
                        }
                    }
                },
                emphasis: {                 // Ҳ��ѡ����ʽ
                    areaStyle: {
                        color: '#fe994e'
                    },
                    label: {
                        textStyle: {
                            color: 'rgb(100,0,0)'
                        }
                    }
                }
            }
        },

        force : {
            itemStyle: {
                normal: {
                    linkStyle : {
                        color : '#27727B'
                    }
                }
            }
        },

        chord : {
            itemStyle : {
                normal : {
                    borderWidth: 1,
                    borderColor: 'rgba(128, 128, 128, 0.5)',
                    chordStyle : {
                        lineStyle : {
                            color : 'rgba(128, 128, 128, 0.5)'
                        }
                    }
                },
                emphasis : {
                    borderWidth: 1,
                    borderColor: 'rgba(128, 128, 128, 0.5)',
                    chordStyle : {
                        lineStyle : {
                            color : 'rgba(128, 128, 128, 0.5)'
                        }
                    }
                }
            }
        },

        gauge : {
            center:['50%','70%'],
            radius:'100%',
            startAngle: 180,
            endAngle : 0,
            axisLine: {            // ��������
                show: true,        // Ĭ����ʾ������show������ʾ���
                lineStyle: {       // ����lineStyle����������ʽ
                    color: [[0.2, '#B5C334'],[0.8, '#27727B'],[1, '#C1232B']],
                    width: '80%'
                }
            },
 
            detail : {
                offsetCenter: [0, 0],       // x, y����λpx
                textStyle: {       // ��������Ĭ��ʹ��ȫ���ı���ʽ�����TEXTSTYLE
                    color: 'auto',
                    fontSize: 20
                }
            }
        }
    };

    return theme;
}