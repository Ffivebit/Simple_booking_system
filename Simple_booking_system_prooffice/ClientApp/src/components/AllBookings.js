import React, { Component } from 'react';

export class AllBookings extends Component {
  static displayName = AllBookings.name;

  constructor(props) {
    super(props);
    this.state = { data: [] };
  }

  componentDidMount() {
    this.populateAllTodayBooking();
  }

  static renderAllBookings(data) {
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
      : AllBookings.renderAllBookings(this.state.data);

    return (
      <div>
        <h1 id="tabelLabel" >All Bookings</h1>
        <p>This is all the bookings that happened.</p>
        {contents}
      </div>
    );
  }

  async populateAllTodayBooking() {
    const response = await fetch('api/Booking');
    const data = await response.json();
    const allBookings = data.map(resource => {
      return { ...resource };
    });
    this.setState({ data: allBookings });
  }
}
