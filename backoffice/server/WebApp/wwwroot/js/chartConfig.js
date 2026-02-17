function defaultBarChartConfig(containerId, data, chartOptions, nonDate, xpercent) {
    nv.addGraph(function () {
        var chart;
        chart = nv.models.multiBarChart()
                .margin({ bottom: 100 })
        ;

        chart.options(chartOptions);
        chart.multibar
            .hideable(true);

        if (nonDate == undefined || !nonDate) {
            chart.xAxis
                // .axisLabel("Current Index")
                .showMaxMin(true)
                .tickFormat(function (d) { return d3.time.format('%x')(new Date(d)) }); //d3.format(',0f'));
        }

        if (xpercent == undefined || !xpercent) {
            chart.yAxis
                .tickFormat(d3.format(',.1f'));
        } else {
            chart.yAxis
                .tickFormat(d3.format('.2%'));
        }

        d3.select('#' + containerId + ' svg')
            .datum(data)
            .call(chart);

        nv.utils.windowResize(chart.update);

        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });

        return chart;
    });
}

function barChartCustomRange(containerId, data, maxValue, minValue, chartOptions, nonDate, xpercent) {
    nv.addGraph(function () {
        var chart;
        chart = nv.models.multiBarChart()
                .margin({ bottom: 100 })
        ;

        chart.options(chartOptions);
        chart.multibar
            .hideable(true);

        if (nonDate == undefined || !nonDate) {
            chart.xAxis
                // .axisLabel("Current Index")
                .showMaxMin(true)
                .tickFormat(function (d) { return d3.time.format('%x')(new Date(d)) }); //d3.format(',0f'));
        }

        if (xpercent == undefined || !xpercent) {
            chart.yAxis
                .tickFormat(d3.format(',.1f'));
        } else {
            var yScale = d3.scale.linear().domain([minValue, maxValue]);
            chart.yAxis
                 .scale(yScale)
                .tickFormat(d3.format('.2%'));
        }

        d3.select('#' + containerId + ' svg')
            .datum(data)
            .call(chart);

        nv.utils.windowResize(chart.update);

        
        chart.forceY([minValue, maxValue]);
        return chart;
    });
}

var colors = d3.scale.category20();
var keyColor = function (d, i) { return colors(d.key) };
function defaultAreaChartConfig(container, data, useGuideline) {
    if (useGuideline === undefined) useGuideline = true;
    nv.addGraph(function () {
        var chart;
        chart = nv.models.stackedAreaChart()
                      .useInteractiveGuideline(useGuideline)
                      .x(function (d) { return d[0] })
                      .y(function (d) { return d[1] })
                      .color(keyColor);

        chart.xAxis
            .tickFormat(function (d) { return d3.time.format('%x')(new Date(d)) });

        chart.yAxis
            .tickFormat(d3.format(',.2f'));

        d3.select('#' + container + ' svg')
              .datum(data)
            .transition().duration(500).call(chart);

        nv.utils.windowResize(chart.update);

        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });

        return chart;
    });
}

function barchartwithline(chartdivid, testdata) {
    nv.addGraph(function () {
        var chart = nv.models.linePlusBarChart()
            .margin({ top: 30, right: 60, bottom: 50, left: 70 })
            .x(function (d, i) { return i })
            .color(d3.scale.category10().range());

        chart.xAxis.tickFormat(function (d) {

            var dx = testdata[0].values[d] && testdata[0].values[d].x || 0;
            if (dx > 0) {
                return d3.time.format('%x')(new Date(dx));
            }
            return null;
        });

        chart.x2Axis.tickFormat(function (d) {
            var dx = testdata[0].values[d] && testdata[0].values[d].x || 0;
            return d3.time.format('%x')(new Date(dx));
        });

        chart.y1Axis
            .tickFormat(d3.format(',f'));

        chart.y3Axis
            .tickFormat(d3.format(',f'));

        chart.y2Axis
            .tickFormat(function (d) { return d3.format(',.2f')(d) });

        chart.y4Axis
            .tickFormat(function (d) { return d3.format(',.2f')(d) });

        chart.bars.forceY([0]);
        chart.bars2.forceY([0]);
        //chart.lines.forceY([0]);
        nv.log(testdata);
        d3.select('#' + chartdivid + ' svg')
            .datum(testdata)
            .call(chart);

        // nv.utils.windowResize(chart.update);
        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });

        return chart;
    });
}

function defaultLineChartConfig(containerid, data, guideline, useDates, auxOptions, chartOptions) {
    if (auxOptions === undefined) auxOptions = {};
    if (guideline === undefined) guideline = true;
    nv.addGraph(function () {
        var chart;
        chart = nv.models.lineChart()
            .useInteractiveGuideline(guideline)
            .margin({ bottom: 100 });



        chart
            .x(function (d, i) {
                return d.x;
            });

        if (auxOptions.width)
            chart.width(auxOptions.width);

        if (auxOptions.height)
            chart.height(auxOptions.height);

        if (auxOptions.forceY)
            chart.forceY([0]);

        var formatter;
        if (useDates !== undefined && useDates) {
            formatter = function (d) { return d3.time.format('%x')(new Date(d)) }
        }
        else {
            formatter = d3.format("");
        }
        chart.margin({ right: 40 });
        chart.xAxis // chart sub-models (ie. xAxis, yAxis, etc) when accessed directly, return themselves, not the parent chart, so need to chain separately
            .tickFormat(
                formatter
              );
        // .tickFormat(function (d) { return d3.time.format('%x')(new Date(d)) });

        chart.yAxis

            //.orient('right')
            //.axisLabel('Voltage (v)')
            .tickFormat(d3.format(',.2f'));

        chart.options(chartOptions);

        d3.select('#' + containerid + ' svg')
            .datum(data)
          .transition().duration(500)
            .call(chart);

        nv.utils.windowResize(chart.update);

        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });

        return chart;
    });
}


function lineChartCustomRange(containerid, data, maxValue, minValue, guideline, useDates, auxOptions, chartOptions) {
    if (auxOptions === undefined) auxOptions = {};
    if (guideline === undefined) guideline = true;
    nv.addGraph(function () {
        var chart = nv.models.lineChart()
            .useInteractiveGuideline(guideline)
            .margin({ bottom: 100 });



        chart
            .x(function (d, i) {
                return d.x;
            });

        if (auxOptions.width)
            chart.width(auxOptions.width);

        if (auxOptions.height)
            chart.height(auxOptions.height);

        if (auxOptions.forceY)
            chart.forceY([0]);

        var formatter;
        if (useDates !== undefined && useDates) {
            formatter = function (d) { return d3.time.format('%x')(new Date(d)) }
        }
        else {
            formatter = d3.format("");
        }
        chart.margin({ right: 40 });
        chart.xAxis // chart sub-models (ie. xAxis, yAxis, etc) when accessed directly, return themselves, not the parent chart, so need to chain separately
            .tickFormat(
                formatter
              );
        // .tickFormat(function (d) { return d3.time.format('%x')(new Date(d)) });
        //   chart.yDomain([-2500, 2500]);
        var yScale = d3.scale.linear().domain([minValue, maxValue]);
        chart.yAxis
            .scale(yScale)
            //.orient('right')
            //.axisLabel('Voltage (v)')
            .tickFormat(d3.format(',.2f'));
        chart.forceY([minValue, maxValue]);
        chart.options(chartOptions);

        d3.select('#' + containerid + ' svg')
            .datum(data)
          .transition().duration(500)
            .call(chart);

        nv.utils.windowResize(chart.update);

        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });

        return chart;
    });
}

function InteractiveTree(containerId, data) {


    var margin = { top: 20, right: 120, bottom: 20, left: 120 },
           width = 960 - margin.right - margin.left,
           height = 500 - margin.top - margin.bottom;

    var i = 0,
        duration = 750,
        root;

    var tree = d3.layout.tree()
        .size([height, width]);

    var diagonal = d3.svg.diagonal()
        .projection(function (d) { return [d.y, d.x]; });

    var svg = d3.select('#' + containerId + " svg")
        .attr("width", width + margin.right + margin.left)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    root = data;
    root.x0 = height / 2;
    root.y0 = 0;
    maxLinkSize = 24;
    minLinkSize = 2;

    maxValue = getMax();
    minValue = getMin();
    function getMax() {
        maxValue = 0;
        findMax();
        function findMax() {
            for (i = 0; i < data.children.length; i++) {
                if (maxValue < data.children[i].value)
                    maxValue = data.children[i].value;
            }
        };
        return maxValue;
    };


    function getMin() {

        minValue = 9e100;
        recursion(data);
        function recursion(obj) {
            if (obj.hasOwnProperty("children") && obj.children != null && obj.children.length > 0) {

                obj.children.forEach(function (child) {
                    recursion(child);
                });
            } else if (obj.hasOwnProperty("value")) {
                if (obj.value < minValue) {
                    minValue = obj.value;
                }
            }
        }
        return minValue;
    };
    linkScale = d3.scale.linear()
        .domain([minValue, maxValue])
        .range([minLinkSize, maxLinkSize]);


    function update(source) {

        // Compute the new tree layout.
        var nodes = tree.nodes(root).reverse(),
            links = tree.links(nodes);

        // Normalize for fixed-depth.
        nodes.forEach(function (d) { d.y = d.depth * 180; });

        // Update the nodes…
        var node = svg.selectAll("g.node")
            .data(nodes, function (d) { return d.id || (d.id = ++i); });

        // Enter any new nodes at the parent's previous position.
        var nodeEnter = node.enter().append("g")
            .attr("class", "node")
            .attr("transform", function (d) { return "translate(" + source.y0 + "," + source.x0 + ")"; })
            .on("click", click);

        nodeEnter.append("circle")
            .attr("r", 1e-6)
            .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

        nodeEnter.append("text")
            .attr("x", function (d) { return d.children || d._children ? -13 : 13; })
            .attr("dy", ".35em")
            .attr("text-anchor", function (d) { return d.children || d._children ? "end" : "start"; })
            .text(function (d) { return d.description + "(" + d.value + ")"; })
            .style("fill-opacity", 1e-6);

        // Transition nodes to their new position.
        var nodeUpdate = node.transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; });

        nodeUpdate.select("circle")
            .attr("r", 10)
            .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

        nodeUpdate.select("text")
            .style("fill-opacity", 1);

        // Transition exiting nodes to the parent's new position.
        var nodeExit = node.exit().transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + source.y + "," + source.x + ")"; })
            .remove();

        nodeExit.select("circle")
            .attr("r", 1e-6);

        nodeExit.select("text")
            .style("fill-opacity", 1e-6);

        // Update the links…
        var link = svg.selectAll("path.link")
            .data(links, function (d) { return d.target.id; });

        // Enter any new links at the parent's previous position.
        link.enter().insert("path", "g")
            .attr("class", "link")
            .attr("style", function (d) {
                var s = linkScale(d.target.value);

                return "stroke-width: " + s + "px";
            })
            .attr("d", function (d) {
                var o = { x: source.x0, y: source.y0 };
                return diagonal({ source: o, target: o });
            });

        // Transition links to their new position.
        link.transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.
        link.exit().transition()
            .duration(duration)
            .attr("d", function (d) {
                var o = { x: source.x, y: source.y };
                return diagonal({ source: o, target: o });
            })
            .remove();

        // Stash the old positions for transition.
        nodes.forEach(function (d) {
            d.x0 = d.x;
            d.y0 = d.y;
        });
    }


    update(root);

    d3.select(self.frameElement).style("height", "2500px");
    // Toggle children on click.
    function click(d) {
        if (d.children) {
            d._children = d.children;
            d.children = null;
        } else {
            d.children = d._children;
            d._children = null;
        }
        update(d);
    }


}
var defaults = {
    margin: { top: 24, right: 0, bottom: 0, left: 0 },
    rootname: "TOP",
    format: ",d",
    title: "",
    width: 960,
    height: 500
};
function mainTreeMap(containerId, o, data) {
    var root,
     opts = $.extend(true, {}, defaults, o),
     formatNumber = d3.format(opts.format),
     rname = opts.rootname,
     margin = opts.margin,
     theight = 36 + 16;

    //////
    var $cont = $('#' + containerId);
    var widthp = $cont.width();
    var heightp = $cont.height();
    if (heightp < widthp)
        heightp = widthp * 0.5;
    var width = widthp == undefined || !parseInt(widthp) ? opts.width : widthp;
    var height = heightp == undefined || !parseInt(heightp) ? opts.height : heightp;
    /////

    //$('#' + containerId).width(opts.width).height(opts.height);
    var width = opts.width - margin.left - margin.right,
        height = opts.height - margin.top - margin.bottom - theight,
        transitioning;

    var color = d3.scale.category20c();

    var x = d3.scale.linear()
        .domain([0, width])
        .range([0, width]);

    var y = d3.scale.linear()
        .domain([0, height])
        .range([0, height]);

    var treemap = d3.layout.treemap()
        .children(function (d, depth) { return depth ? null : d._children; })
        .sort(function (a, b) { return a.value - b.value; })
        .ratio(height / width * 0.5 * (1 + Math.sqrt(5)))
        .round(false);

    var svg = d3.select('#' + containerId).append("svg")
        //.attr("width", width + margin.left + margin.right)
        //.attr("height", height + margin.bottom + margin.top)
        .attr("width", '100%')
            .attr("height", '100%')
            .attr('viewBox', '0 0 ' + width + ' ' + height)
        .attr("preserveAspectRatio", "xMinYMin meet")
        .style("margin-left", -margin.left + "px")
        .style("margin.right", -margin.right + "px")
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")")
        .style("shape-rendering", "crispEdges");

    var grandparent = svg.append("g")
        .attr("class", "grandparent");

    grandparent.append("rect")
        .attr("y", -margin.top)
        .attr("width", width)
        .attr("height", margin.top);

    grandparent.append("text")
        .attr("x", 6)
        .attr("y", 6 - margin.top)
        .attr("dy", ".75em");


    if (data instanceof Array) {
        root = { key: rname, values: data };
    } else {
        root = data;
    }

    initialize(root);
    accumulate(root);
    layout(root);
    display(root);

    if (window.parent !== window) {
        var myheight = document.documentElement.scrollHeight || document.body.scrollHeight;
        window.parent.postMessage({ height: myheight }, '*');
    }

    function initialize(root) {
        root.x = root.y = 0;
        root.dx = width;
        root.dy = height;
        root.depth = 0;
    }

    // Aggregate the values for internal nodes. This is normally done by the
    // treemap layout, but not here because of our custom implementation.
    // We also take a snapshot of the original children (_children) to avoid
    // the children being overwritten when when layout is computed.
    function accumulate(d) {
        return (d._children = d.values)
            ? d.value = d.values.reduce(function (p, v) { return p + accumulate(v); }, 0)
            : d.value;
    }

    // Compute the treemap layout recursively such that each group of siblings
    // uses the same size (1×1) rather than the dimensions of the parent cell.
    // This optimizes the layout for the current zoom state. Note that a wrapper
    // object is created for the parent node for each group of siblings so that
    // the parent’s dimensions are not discarded as we recurse. Since each group
    // of sibling was laid out in 1×1, we must rescale to fit using absolute
    // coordinates. This lets us use a viewport to zoom.
    function layout(d) {
        if (d._children) {
            treemap.nodes({ _children: d._children });
            d._children.forEach(function (c) {
                c.x = d.x + c.x * d.dx;
                c.y = d.y + c.y * d.dy;
                c.dx *= d.dx;
                c.dy *= d.dy;
                c.parent = d;
                layout(c);
            });
        }
    }

    function display(d) {
        grandparent
            .datum(d.parent)
            .on("click", transition)
          .select("text")
            .text(name(d));

        var g1 = svg.insert("g", ".grandparent")
            .datum(d)
            .attr("class", "depth");

        var g = g1.selectAll("g")
            .data(d._children)
          .enter().append("g");

        g.filter(function (d) { return d._children; })
            .classed("children", true)
            .on("click", transition);

        var children = g.selectAll(".child")
            .data(function (d) { return d._children || [d]; })
          .enter().append("g");

        children.append("rect")
            .attr("class", "child")
            .call(rect)
          .append("title")
            .text(function (d) { return d.key + " (" + formatNumber(d.value) + ")"; });
        children.append("text")
            .attr("class", "ctext")
            .text(function (d) { return d.key; })
            .call(text2);

        g.append("rect")
            .attr("class", "parent")
            .call(rect);

        var t = g.append("text")
            .attr("class", "ptext")
            .attr("dy", ".75em");

        t.append("tspan")
            .text(function (d) { return d.key; });
        t.append("tspan")
            .attr("dy", "1.0em")
            .text(function (d) { return formatNumber(d.value); });
        t.call(text);

        g.selectAll("rect")
            .style("fill", function (d) { return color(d.key); });

        function transition(d) {
            if (transitioning || !d) return;
            transitioning = true;

            var g2 = display(d),
                t1 = g1.transition().duration(750),
                t2 = g2.transition().duration(750);

            // Update the domain only after entering new elements.
            x.domain([d.x, d.x + d.dx]);
            y.domain([d.y, d.y + d.dy]);

            // Enable anti-aliasing during the transition.
            svg.style("shape-rendering", null);

            // Draw child nodes on top of parent nodes.
            svg.selectAll(".depth").sort(function (a, b) { return a.depth - b.depth; });

            // Fade-in entering text.
            g2.selectAll("text").style("fill-opacity", 0);

            // Transition to the new view.
            t1.selectAll(".ptext").call(text).style("fill-opacity", 0);
            t1.selectAll(".ctext").call(text2).style("fill-opacity", 0);
            t2.selectAll(".ptext").call(text).style("fill-opacity", 1);
            t2.selectAll(".ctext").call(text2).style("fill-opacity", 1);
            t1.selectAll("rect").call(rect);
            t2.selectAll("rect").call(rect);

            // Remove the old node when the transition is finished.
            t1.remove().each("end", function () {
                svg.style("shape-rendering", "crispEdges");
                transitioning = false;
            });
        }

        return g;
    }

    function text(text) {
        text.selectAll("tspan")
            .attr("x", function (d) { return x(d.x) + 6; })
        text.attr("x", function (d) { return x(d.x) + 6; })
            .attr("y", function (d) { return y(d.y) + 6; })
            .style("opacity", function (d) { return this.getComputedTextLength() < x(d.x + d.dx) - x(d.x) ? 1 : 0; });
    }

    function text2(text) {
        text.attr("x", function (d) { return x(d.x + d.dx) - this.getComputedTextLength() - 6; })
            .attr("y", function (d) { return y(d.y + d.dy) - 6; })
            .style("opacity", function (d) { return this.getComputedTextLength() < x(d.x + d.dx) - x(d.x) ? 1 : 0; });
    }

    function rect(rect) {
        rect.attr("x", function (d) { return x(d.x); })
            .attr("y", function (d) { return y(d.y); })
            .attr("width", function (d) { return x(d.x + d.dx) - x(d.x); })
            .attr("height", function (d) { return y(d.y + d.dy) - y(d.y); });
    }

    function name(d) {
        return d.parent
            ? name(d.parent) + " / " + d.key + " (" + formatNumber(d.value) + ")"
            : d.key + " (" + formatNumber(d.value) + ")";
    }
}
function defaultPieChart(containerId, data, labelType, showLegend, noDecimal = false) {
    if (showLegend == undefined || showLegend == null) {
        showLegend = true;
    }
    nv.addGraph(function() {
        var $cont = $('#' + containerId);
       var widthp = $cont.width();
     var   heightp = $cont.height();
        if (heightp < widthp)
            heightp = widthp * 0.87;
        var width = widthp == undefined || !parseInt(widthp) ? 650 : widthp;
        var height = heightp == undefined || !parseInt(heightp) ? 565 : heightp;
        var chart = nv.models.pieChart()
            .x(function(d) { return d.key })
            .y(function(d) { return d.y })
            .color(d3.scale.category10().range())
            .width(width)
            .height(height)
            .labelType(labelType)
            .showLegend(showLegend);

        if (noDecimal == true) {
            chart.tooltip.valueFormatter(function (d) { return d.toFixed(0) });
        }

        d3.select("#" + containerId + " svg")
            .datum(data)
            .transition().duration(600)
            .attr("width", '100%')
            .attr("height", '100%')
            .attr('viewBox', '0 0 ' + width + ' ' + height)
            .attr('preserveAspectRatio', 'xMinYMin')
           // .attr("transform", "translate(" + Math.min(width, height) / 2 + "," + Math.min(width, height) / 2 + ")")
            .call(chart);

        nv.utils.windowResize(chart.update);
        return chart;
    });


}



function tabulate(data, container, columns) {
    var parent = d3.select("#" + container);

    var table = d3.select("#" + container).append("table"),
        thead = table.append("thead"),
        tbody = table.append("tbody");

    // append the header row
    thead.append("tr")
        .selectAll("th")
        .data(columns)
        .enter()
        .append("th")
            .text(function (column) { return column; });

    // create a row for each object in the data
    var rows = tbody.selectAll("tr")
        .data(data)
        .enter()
        .append("tr");

    // create a cell in each row for each column
    var cells = rows.selectAll("td")
        .data(function (row) {
            return columns.map(function (column) {
                return { column: column, value: row[column] };
            });
        })
        .enter()
        .append("td")
            .text(function (d) { return d.value; });

    return table;
}

function defaultSactter(container, data) {

    nv.addGraph(function () {
        var chart = nv.models.scatterChart()
            .showDistX(true)
            .showDistY(true)
            .pointRange([500, 2500])
            .duration(300)
            .color(d3.scale.category10().range());

        chart.dispatch.on('renderEnd', function () {
            console.log('render complete');
        });

        //chart.xAxis.tickFormat(d3.format('.02f'));
        chart.yAxis.tickFormat(d3.format('.02f'));
        d3.select('#' + container + ' svg')
            .datum(nv.log(data))
            .call(chart);
        nv.utils.windowResize(chart.update);
        chart.dispatch.on('stateChange', function (e) { nv.log('New State:', JSON.stringify(e)); });
        return chart;
    });
}

