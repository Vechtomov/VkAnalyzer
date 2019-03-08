import React from 'react';
import PropTypes from 'prop-types';

// Pond
import { TimeSeries, TimeRangeEvent, TimeRange } from 'pondjs';

// Imports from the charts library
import {
  ChartContainer,
  ChartRow,
  Charts,
  EventChart,
  Resizable,
} from 'react-timeseries-charts';

function outageEventStyleFunc(event, state) {
  const color = event._d.getIn(['data', 'type']) === 2 ? 'blue' : 'green';
  switch (state) {
    case 'normal':
      return {
        fill: color,
      };
    case 'hover':
      return {
        fill: color,
        opacity: 0.4,
      };
    case 'selected':
      return {
        fill: color,
      };
    default:
      return {};
  }
}

class OnlineChart extends React.Component {
  state = {
    timerange: null,
  };

  handleTimeRangeChange = timerange => {
    this.setState({ timerange });
  };

  mapOnlineInfosToSeries = ranges => {
    const timeRanges = ranges.map(
      ({ startTime, endTime, ...data }) =>
        new TimeRangeEvent(
          new TimeRange(new Date(startTime), new Date(endTime)),
          data,
        ),
    );

    const series = new TimeSeries({ name: 'online', events: timeRanges });
    return series;
  };

  setSeries = series => {
    this.setState({ timerange: series.timerange() });
  };

  render() {
    let { timerange } = this.state;
    const { data } = this.props;

    if (!data) return null;

    if (data.length === 0)
      return (
        <div>В указанный промежуток времени пользователь не был в сети.</div>
      );

    const series = this.mapOnlineInfosToSeries(data);
    timerange = timerange || series.timerange();
    return (
      <div>
        <Resizable>
          <ChartContainer
            timeRange={timerange}
            enablePanZoom
            onTimeRangeChanged={this.handleTimeRangeChange}
          >
            <ChartRow height="60">
              <Charts>
                <EventChart
                  series={series}
                  size={45}
                  style={outageEventStyleFunc}
                  // label={e => e.get('value')}
                />
              </Charts>
            </ChartRow>
          </ChartContainer>
        </Resizable>
      </div>
    );
  }
}

OnlineChart.propTypes = {
  data: PropTypes.array,
};

export default OnlineChart;
