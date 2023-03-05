import React, { Component } from 'react';

export class TodayBookings extends Component {
  static displayName = TodayBookings.name;

  constructor(props) {
    super(props);
    this.state = { data: []};
  }

  componentDidMount() {
    this.populateAllTodayBooking();
  }
  static renderAllTodayBookings(data) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date From</th>
            <th>Date To</th>
            <th>Booked Quantity</th>
            <th>Resource ID</th>
          </tr>
        </thead>
        <tbody>
          {data.map(data =>
            <tr key={data.id}>
              <td>{data.dateFrom}</td>
              <td>{data.dateTo}</td>
              <td>{data.bookedQuantity}</td>
              <td>{data.resourceId}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
}

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : TodayBookings.renderAllTodayBookings(this.state.data);

    return (
      <div>
        <h1 id="tabelLabel" >Today Bookings</h1>
        <p>This is all of today's bookings that happened.</p>
        {contents}
      </div>
    );
  }

  async populateAllTodayBooking() {
    const response = await fetch('api/Booking/GetTodayBookings');
    const data = await response.json();
    const resourcesWithDataToday = data.map(resource => {
      return { ...resource };
    });
    this.setState({ data: resourcesWithDataToday });
  }
}