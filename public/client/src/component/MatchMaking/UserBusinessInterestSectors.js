import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "../ServiceRequest/ServiceRequestDetailTbl.css";
import { Button, Col, Form, Row, Table, InputGroup } from "react-bootstrap";
import {
  getChildSectors,
  getRootSectors,
} from "../../services/userBusinessInterest";

export default function UserBusinessInterestSectors({
  userSectors,
  setUserSectors,
  setError,
  setErrorMsg,
  showNotification,
}) {
  const [sectors, setSectors] = React.useState([]);
  const [filteredSectors, setFilteredSectors] = React.useState([]);
  const [selectedSectorId, setSelectedSectorId] = React.useState(null);
  const [isRootSectors, setIsRootSectors] = React.useState(true);
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    if (isRootSectors) {
      setSectors([]);
      setLoading(true);
      getRootSectors()
        .then((data) => {
          setLoading(false);
          setSectors(data.data.$values);
        })
        .catch((error) => {
          setLoading(false);
        });
    } else {
      if (!checkSectorSelected()) return;
      setSectors([]);
      setLoading(true);
      getChildSectors(selectedSectorId)
        .then((data) => {
          setLoading(false);
          setSectors(data.data.$values);
        })
        .catch((error) => {
          setLoading(false);
        });
    }
  }, [isRootSectors]);
  React.useEffect(() => {
    setFilteredSectors([]);
    if (sectors) {
      setFilteredSectors(
        sectors.filter((s) => !userSectors.find((u) => u.sectorId == s.id))
      );
    }
  }, [userSectors, sectors]);

  React.useEffect(() => {
    if (filteredSectors && filteredSectors.length > 0) {
      setSelectedSectorId(filteredSectors[0].id);
    }
  }, [filteredSectors]);
  //
  const checkSectorSelected = () => {
    if (!selectedSectorId) {
      setErrorMsg("Please select sector!");
      setError(true);
      showNotification("tr");
      return false;
    }
    return true;
  };
  //
  const AddSector = (e) => {
    if (isRootSectors) {
      return;
    }
    const selectedSector = sectors.find((s) => s.id == selectedSectorId);
    if (!checkSectorSelected()) return;
    if (!selectedSector) return;
    if (userSectors.find((u) => u.sectorId == selectedSectorId)) return;
    const newSector = { sectorId: selectedSectorId, name: selectedSector.name };
    const newList = [...userSectors];
    newList.push(newSector);
    setUserSectors(newList);
  };
  const RemoveSector = (ss) => {
    const newList = [...userSectors.filter((s) => s.sectorId != ss.sectorId)];
    setUserSectors(newList);
  };
  return (
    <>
      <strong>Sectors</strong> &nbsp;&nbsp;&nbsp;
      <Row>
        <Col>
          <InputGroup>
            <InputGroup.Text>Sectors</InputGroup.Text>
            <Form.Select
              name="sector"
              style={{ maxWidth: "500px" }}
              value={selectedSectorId}
              onChange={(e) => setSelectedSectorId(e.target.value)}
            >
              {filteredSectors.map((value) => (
                <option
                  value={value.id}
                  selected={selectedSectorId == value.id}
                >
                  {value.name}
                </option>
              ))}
            </Form.Select>
            <Button
              type="button"
              size="sm"
              style={{ float: "none", margin: "0px", zIndex: "0" }}
              variant="outline-success"
              disabled={isRootSectors}
              onClick={() => AddSector()}
            >
              Add
            </Button>
            <Button
              type="button"
              size="sm"
              style={{ float: "none", margin: "0px", zIndex: "0" }}
              variant="outline-secondary"
              onClick={() => setIsRootSectors(!isRootSectors)}
              disabled={loading}
            >
              {loading ? (
                <strong> (Loading ...)</strong>
              ) : (
                <>
                  {isRootSectors ? "Get Sub Sectors" : "Back To Root Sectors"}
                </>
              )}
            </Button>
          </InputGroup>
        </Col>{" "}
      </Row>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>Sector</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {userSectors.length > 0 ? (
            userSectors.map((ss) => (
              <tr>
                <td>{ss.name}</td>
                <td>
                  <button
                    type="button"
                    class="btn-sm btn btn-warning "
                    style={{ float: "none", margin: "0" }}
                    onClick={() => RemoveSector(ss)}
                  >
                    {" "}
                    Remove
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={2} className="text-center">
                <strong>No Sector to show</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
