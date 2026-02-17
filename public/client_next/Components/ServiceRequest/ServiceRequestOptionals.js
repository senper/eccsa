import React from 'react';
import { Card, Col, Row, Table } from 'react-bootstrap';
import {
  deleteOptionalEntry,
  saveOptionalEntry,
} from '../../api/serviceRequest';

export default function ServiceRequestOptionals({
  loadRequest,
  serviceType,
  optionalEntries,
  optionalEntryLookupValues,
  showNotification,
  setErrorMsg,
  requestId,
  showUpdateButtons,
  getLocal,
  getLocalByVal,
}) {
  const Save = (e) => {
    e.preventDefault();
    var formData = new FormData();
    formData.append('Label', e.target[0].value);
    formData.append('Value', e.target[1].value);
    formData.append('ServiceTypeId', serviceType.id);
    formData.append('RequestId', requestId);
    saveOptionalEntry(formData)
      .then((data) => {
        if (data.data.saved) {
          loadRequest();
          e.target[0].value = '';
          e.target[1].value = '';
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification('tr');
        }
      })
      .catch((error) => {
        // console.log(error.response);
        showNotification('tr');
      });
  };
  const RemoveOptionalEntry = (att) => {
    deleteOptionalEntry(att.label, requestId)
      .then((data) => {
        if (data.data.success) {
          loadRequest();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification('tr');
        }
      })
      .catch((error) => {
        // console.log(error.response);
        showNotification('tr');
      });
  };
  return (
    <>
      {showUpdateButtons ? (
        <>
          <strong>{getLocal('Add Optional Requirements')}</strong> &nbsp;&nbsp;&nbsp;
          <form onSubmit={(e) => Save(e)}>
            <Row>
              <Col md="6">
                Label:{' '}
                <select
                  name={'optionalEntryLabel'}
                  required
                  className="form-control"
                >
                  {optionalEntryLookupValues
                  ? optionalEntryLookupValues.map((lp, index) => (
                      <option key={index} value={lp.value}>
                        {lp.name}
                      </option>
                    ))
                  : null}
              </select>
              </Col>
              <Col md="4">
                {' '}
                {getLocal('Value')}:{' '}
                <input
                  type="text"
                  name={'optionalEntryValue'}
                  required
                  className="form-control"
                />
              </Col>
              <Col md="2">
                <button
                  type="submit"
                  className="btn-sm btn btn-warning"
                  style={{ float: 'none' }}
                >
                  {getLocal('Save')}
                </button>
              </Col>{' '}
            </Row>
          </form>
        </>
      ) : null}
      <div id="showOnLargeScreen">
        <Table striped bordered hover size="sm" className="three-dtbl">
          <thead>
            <tr>
              <th>{getLocal('Label')}</th>
              <th>{getLocal('Value')}</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {optionalEntries.length > 0 ? (
              optionalEntries.map((att, index) => (
                <tr key={index}>
                  <td>{att.label}</td>
                  <td>{att.value}</td>
                  <td>
                    {showUpdateButtons ? (
                      <button
                        type="button"
                        className="btn-sm btn btn-warning "
                        style={{ float: 'none', margin: '0' }}
                        onClick={() => RemoveOptionalEntry(att)}
                      >
                        {' '}
                        {getLocal('Remove')}
                      </button>
                    ) : null}
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={2} className="text-center">
                  <strong>{getLocal('No Optional Entries to show')}</strong>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      </div>
      <div id="showOnSmallScreen">
        <Card>
          <Card.Header>
            <strong>{getLocal('Optional Requirements')}</strong>
          </Card.Header>
          <Card.Body>
            {optionalEntries.length > 0 ? (
              optionalEntries.map((att, index) => (
                <Row key={index}>
                  <Col>{att.label}</Col>
                  <Col>{att.value}</Col>
                  <Col>
                    {showUpdateButtons ? (
                      <button
                        type="button"
                        className="btn-sm btn btn-warning "
                        style={{ float: 'none', margin: '0' }}
                        onClick={() => RemoveOptionalEntry(att)}
                      >
                        {' '}
                        {getLocal('Remove')}
                      </button>
                    ) : null}
                  </Col>
                </Row>
              ))
            ) : (
              <strong>{getLocal('No Optional Entries to show')}</strong>
            )}
          </Card.Body>
        </Card>
      </div>
    </>
  );
}
