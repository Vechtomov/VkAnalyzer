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
    title: 'ANL Scheduled Maintenance',
    description: 'ANL will be switching border routers...',
    completed: true,
    external_ticket: '',
    esnet_ticket: 'ESNET-20150302-002',
    organization: 'ANL',
    type: 'Planned',
  },
  {
    startTime: '2015-04-01T03:30:00Z',
    endTime: '2015-04-02T16:50:00Z',
    title: 'STAR-CR5 < 100 ge 06519 > ANL  - Outage',
    description: 'The listed circuit was unavailable due to bent pins.',
    completed: true,
    external_ticket: '3576:144',
    esnet_ticket: 'ESNET-20150421-013',
    organization: 'Internet2 / Level 3',
    type: 'Unplanned',
  },
  {
    startTime: '2015-04-22T03:30:00Z',
    endTime: '2015-04-22T13:00:00Z',
    description: 'At 13:33 pacific circuit 06519 went down.',
    title: 'STAR-CR5 < 100 ge 06519 > ANL  - Outage',
    completed: true,
    external_ticket: '',
    esnet_ticket: 'ESNET-20150421-013',
    organization: 'Internet2 / Level 3',
    type: 'Unplanned',
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
            <ChartRow height="30">
              <Charts>
                <EventChart
                  series={series}
                  size={45}
                  style={outageEventStyleFunc}
                  label={e => e.get('title')}
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
