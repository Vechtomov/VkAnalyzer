import React from 'react';

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

//
// Test data
//

const outageEvents = [
  {
    startTime: '2015-03-08T09:00:00Z',
    endTime: '2015-03-22T14:00:00Z',
    type: 'OnlineMobile',
  },
  {
    startTime: '2015-04-01T03:30:00Z',
    endTime: '2015-04-02T16:50:00Z',
    type: 'Offline',
  },
  {
    startTime: '2015-04-23T03:30:00Z',
    endTime: '2015-04-23T13:00:00Z',
    type: 'Online',
  },
  {
    startTime: '2015-04-24T03:30:00Z',
    endTime: '2015-04-24T16:50:00Z',
    type: 'Offline',
  },
  {
    startTime: '2015-04-25T03:30:00Z',
    endTime: '2015-04-25T13:00:00Z',
    type: 'Online',
  },
  {
    startTime: '2015-04-26T03:30:00Z',
    endTime: '2015-04-26T16:50:00Z',
    type: 'Offline',
  },
  {
    startTime: '2015-04-27T03:30:00Z',
    endTime: '2015-04-27T13:00:00Z',
    type: 'Online',
  },
  {
    startTime: '2015-04-29T03:30:00Z',
    endTime: '2015-04-29T16:50:00Z',
    type: 'Offline',
  },
  {
    startTime: '2015-04-30T03:30:00Z',
    endTime: '2015-04-30T13:00:00Z',
    type: 'Online',
  },
];

//
// Turn data into TimeSeries
//

const events = outageEvents.map(
  ({ startTime, endTime, ...data }) =>
    new TimeRangeEvent(
      new TimeRange(new Date(startTime), new Date(endTime)),
      data,
    ),
);
const series = new TimeSeries({ name: 'outages', events });

//
// Render event chart
//

function outageEventStyleFunc(event, state) {
  const color = event.get('type') === 'Planned' ? '#998ec3' : '#f1a340';
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

class OutAges extends React.Component {
  state = {
    tracker: null,
    timerange: series.timerange(),
  };

  handleTrackerChanged = tracker => {
    this.setState({ tracker });
  };

  handleTimeRangeChange = timerange => {
    this.setState({ timerange });
  };

  render() {
    return (
      <div>
        <Resizable>
          <ChartContainer
            timeRange={this.state.timerange}
            enablePanZoom
            onTimeRangeChanged={this.handleTimeRangeChange}
          >
            <ChartRow height="60">
              <Charts>
                <EventChart
                  series={series}
                  size={45}
                  style={outageEventStyleFunc}
                  // label={e => e.get('title')}
                />
              </Charts>
            </ChartRow>
          </ChartContainer>
        </Resizable>
      </div>
    );
  }
}

// Export example
export default OutAges;
