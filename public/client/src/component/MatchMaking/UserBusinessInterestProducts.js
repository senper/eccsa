import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "../ServiceRequest/ServiceRequestDetailTbl.css";
import { Button, Col, Form, Row, Table, InputGroup } from "react-bootstrap";
import { getLookupValues } from "../../services/contactUs";
import { getBusinessInterestProducts } from "../../services/userBusinessInterest";

export default function UserBusinessInterestProducts({
  userProducts,
  setUserProducts,
  setError,
  setErrorMsg,
  showNotification,
}) {
  const [businessTypes, setBusinessTypes] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getLookupValues("00000000-0000-0000-0000-00000000002e")
      .then((data) => {
        setBusinessTypes(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }, []);
  //
  const [productTypes, setProductTypes] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getBusinessInterestProducts()
      .then((data) => {
        setProductTypes(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }, []);
  const [loading, setLoading] = React.useState(false);
  //
  const checkInputsSelected = (event) => {
    if (!event.target[1].value) {
      setErrorMsg("Please select product type!");
      setError(true);
      showNotification("tr");
      return false;
    }
    if (!event.target[0].value) {
      setErrorMsg("Please select business type!");
      setError(true);
      showNotification("tr");
      return false;
    }
    return true;
  };
  //
  const Add = (event) => {
    event.preventDefault();
    if (!checkInputsSelected(event)) return;
    const selectedProductType = productTypes.find(
      (s) => s.id == event.target[1].value
    );
    if (!selectedProductType) {
      setErrorMsg("Select product type only from the list!");
      setError(true);
      showNotification("tr");
      return;
    }
    if (
      userProducts.find(
        (u) =>
          selectedProductType.id == u.productTypeId &&
          event.target[0].value == u.businessType
      )
    ) {
      setErrorMsg("Product Type for business type already added!");
      setError(true);
      showNotification("tr");
      return;
    }
    const newProductType = {
      productTypeId: selectedProductType.id,
      productName: selectedProductType.name,
      description: event.target[2].value,
      businessType: event.target[0].value,
    };
    const newList = [...userProducts];
    newList.push(newProductType);
    setUserProducts(newList);
  };
  const RemoveProduct = (ss) => {
    const newList = [
      ...userProducts.filter(
        (s) =>
          !(
            s.productTypeId == ss.productTypeId &&
            s.businessType == ss.businessType
          )
      ),
    ];
    setUserProducts(newList);
  };
  return (
    <>
      <strong>Products</strong> &nbsp;&nbsp;&nbsp;
      <form onSubmit={Add}>
        <Row>
          <Col>
            <InputGroup>
              <InputGroup.Text>Business Type</InputGroup.Text>
              <Form.Select name="businessType" style={{ maxWidth: "500px" }}>
                {businessTypes.map((value) => (
                  <option value={value.id}>{value.name}</option>
                ))}
              </Form.Select>
            </InputGroup>
          </Col>
          <Col>
            <InputGroup>
              <InputGroup.Text>Product Type</InputGroup.Text>
              <Form.Select name="productType" style={{ maxWidth: "500px" }}>
                {productTypes.map((value) => (
                  <option value={value.id}>{value.name}</option>
                ))}
              </Form.Select>
            </InputGroup>
          </Col>
        </Row>
        <Row>
          <Col>
            <InputGroup>
              <InputGroup.Text>Description</InputGroup.Text>
              <Form.Control
                as="textarea"
                rows="2"
                name="description"
                required={true}
              />
              <Button
                type="submit"
                size="sm"
                style={{
                  float: "none",
                  margin: "0px",
                  zIndex: "0",
                  height: "35px",
                  width: "80px",
                }}
                variant="outline-success"
                disabled={loading}
              >
                {loading ? "Loading..." : "Add"}
              </Button>
            </InputGroup>
          </Col>
        </Row>
      </form>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>Business Type</th>
            <th>Product Type</th>
            <th>Description</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {!loading && userProducts.length > 0 ? (
            userProducts.map((ss) => (
              <tr>
                <td>
                  {businessTypes.find((b) => b.id == ss.businessType)
                    ? businessTypes.find((b) => b.id == ss.businessType).name
                    : ""}
                </td>
                <td>{ss.productName}</td>
                <td>{ss.description}</td>
                <td>
                  <button
                    type="button"
                    class="btn-sm btn btn-warning "
                    style={{ float: "none", margin: "0" }}
                    onClick={() => RemoveProduct(ss)}
                  >
                    {" "}
                    Remove
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={3} className="text-center">
                <strong>{loading ? "Loading ..." : "No Sector to show"}</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
