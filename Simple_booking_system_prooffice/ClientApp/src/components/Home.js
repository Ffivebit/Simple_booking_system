import React, { Component,useState } from 'react';
import { Table, Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, InputGroup, InputGroupText, Alert } from 'reactstrap';

export class Home extends Component {
  constructor(props) {
    super(props);

    this.state = {
      modalOpen: false,
      bookingData: {
        resourceId: 0,
        dateFrom: '',
        dateTo: '',
        bookedQuantity: 1
      },
      resources: [],
      visible: false,
      alertMessage: '',
      alertColor: ''
    };

    this.book = this.book.bind(this);
  }

  componentDidMount() {
    this.populateResourceData();
  }

  toggleModal = (resource) => {
    this.setState(prevState => ({
      modalOpen: !prevState.modalOpen,
      selectedResource: resource
    }));
  }

  dismissAlert = () => {
    this.setState({ visible: false, alertMessage: '', alertColor: '' });
  }
  

  render() {
    return (
      <div>
        <h2>Welcome to the simple booking system</h2>
        <p>Click on any of the resources to book a timeslot for any of the services you need</p>
          {this.state.alertMessage && (
          <Alert color={this.state.alertColor} isOpen={this.state.visible} toggle={this.dismissAlert}>
            {this.state.alertMessage}
          </Alert>
          )}
        <RequestTable toggleModal={this.toggleModal} modalOpen={this.state.modalOpen} data={this.state.resources} book={this.book}/>
      </div>
    );
  }

  async populateResourceData() {
    const response = await fetch('api/Resources');
    const data = await response.json();
    const resourcesWithData = data.map(resource => {
      return { ...resource };
    });
    this.setState({ resources: resourcesWithData });
  }

  async book(bookingData){
    const request = {
      resourceId: bookingData.resourceId,
      dateFrom: bookingData.dateFrom,
      dateTo: bookingData.dateTo,
      bookedQuantity: bookingData.bookedQuantity
    };

    let invalidDates = validateDates(bookingData.dateFrom,bookingData.dateTo)
    if (invalidDates){
      this.setState({alertMessage:invalidDates, alertColor: 'danger', visible: true})
      return;
    }
    

    const response = await fetch('api/Booking', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(request)
    });

    if (response.ok) {
      // Booking successful
      const message = 'Booking successful!';
      this.setState({ alertMessage: message, alertColor: 'success', visible: true });
    } else {
      // Booking failed
      const error = await response.text();
      const message = `Booking failed: ${error}`;
      this.setState({ alertMessage: message, alertColor: 'danger', visible: true });
    }
  }
  
}

function RequestTable(props) {
  const [bookingData, setBookingData] = useState({
    resourceId: 0,
    dateFrom: '',
    dateTo: '',
    bookedQuantity: 1
  });
  const [allInputsFilled, setAllInputsFilled] = useState(false);
  const handleDateFromChange = (event) => {
    setBookingData({...bookingData, dateFrom: event.target.value});
    const allInputsFilledNow = Boolean(bookingData.dateFrom && bookingData.dateTo && bookingData.bookedQuantity);
    setAllInputsFilled(allInputsFilledNow);

  }

  const handleDateToChange = (event) => {
    setBookingData({...bookingData, dateTo: event.target.value});
    const allInputsFilledNow = Boolean(bookingData.dateFrom && bookingData.dateTo && bookingData.bookedQuantity);
    setAllInputsFilled(allInputsFilledNow);

  }

  const handleQuantityChange = (event) => {
    setBookingData({...bookingData, bookedQuantity: parseInt(event.target.value)});
    const allInputsFilledNow = Boolean(bookingData.dateFrom && bookingData.dateTo && bookingData.bookedQuantity);
    setAllInputsFilled(allInputsFilledNow);

  }

  const handleBook = () => {
    props.book(bookingData);
    props.toggleModal();
  }

  return (
    <Table bordered hover responsive striped>
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Book</th>
        </tr>
      </thead>
      <tbody>
        {props.data.map(resource => (
          <tr key={resource.id}>
            <th scope="row">{resource.id}</th>
            <td>{resource.name}</td>
            <td>
              <div>
                <Button color="success" outline size="" onClick={() => {
                  setBookingData({...bookingData, resourceId: resource.id});
                  props.toggleModal();
                }}>
                  Book Here
                </Button>
              </div>
            </td>
          </tr>
        ))}
      </tbody>

      <Modal isOpen={props.modalOpen} toggle={props.toggleModal}>
        <ModalHeader toggle={props.toggleModal}>Booking {props.name}</ModalHeader>
        <ModalBody>
          <div>
            <DateFromInput onChange={handleDateFromChange} />
            <br />
            <DateToInput onChange={handleDateToChange} />
            <br />
            <QuantityInput onChange={handleQuantityChange} />
            <br />
          </div>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleBook} disabled={!allInputsFilled}>Book</Button>{' '}
          <Button color="secondary" onClick={props.toggleModal}>Cancel</Button>
        </ModalFooter>
      </Modal>
    </Table>
  );
}


function DateFromInput(props) {
  return (
    <InputGroup>
      <InputGroupText>Date From</InputGroupText>
      <Input placeholder="Date From" type="datetime-local" value={props.dateFrom} onChange={props.onChange} required />
    </InputGroup>
  );
}

function DateToInput(props) {
  return (
    <InputGroup>
      <InputGroupText>Date To</InputGroupText>
      <Input placeholder="Date To" type="datetime-local" value={props.dateTo} onChange={props.onChange} required />
    </InputGroup>
  );
}

function QuantityInput(props) {
  return (
    <InputGroup>
      <InputGroupText>Quantity</InputGroupText>
      <Input placeholder="Quantity" type="number" min="1" value={props.bookedQuantity} onChange={props.onChange} required />
    </InputGroup>
  );
}

function validateDates(dateFrom, dateTo) {
  const fromDate = new Date(dateFrom);
  const toDate = new Date(dateTo);

  if (fromDate > toDate) {
    return 'Error: Date From cannot be greater than Date To';
  }

  return null; // indicates no error
}

