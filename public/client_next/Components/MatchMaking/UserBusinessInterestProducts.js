import React from "react";
//import "../ServiceRequest/ServiceRequestDetailTbl.css";
import {
  Button,
  Col,
  Form,
  Row,
  Table,
  InputGroup,
  Card,
} from "react-bootstrap";
import { getLookupValues } from "../../pages/api/contactUs";
import { getBusinessInterestProducts } from "../../api/userBusinessInterest";

export default function UserBusinessInterestProducts({
  userProducts,
  setUserProducts,
  setError,
  setErrorMsg,
  showNotification,
  getLocal,
  getLocalByVal,
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
      setErrorMsg(getLocal("Please select product type!"));
      setError(true);
      showNotification("tr");
      return false;
    }
    if (!event.target[0].value) {
      setErrorMsg(getLocal("Please select business type!"));
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
      setErrorMsg(getLocal("Select product type only from the list!"));
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
      setErrorMsg(getLocal("Product Type for business type already added!"));
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
              <InputGroup.Text>{getLocal("Business Type")}</InputGroup.Text>
              <Form.Select name="businessType" style={{ maxWidth: "500px" }}>
                {businessTypes.map((value, index) => (
                  <option key={index} value={value.id}>
                    {value.name}
                  </option>
                ))}
              </Form.Select>
            </InputGroup>
          </Col>
          <Col>
            <InputGroup>
              <InputGroup.Text>{getLocal("Product Type")}</InputGroup.Text>
              <Form.Select name="productType" style={{ maxWidth: "500px" }}>
                {productTypes.map((value, index) => (
                  <option key={index} value={value.id}>
                    {value.name}
                  </option>
                ))}
              </Form.Select>
            </InputGroup>
          </Col>
        </Row>
        <Row>
          <Col>
            <InputGroup>
              <InputGroup.Text>{getLocal("Description")}</InputGroup.Text>
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
                {loading ? `${getLocal("Loading")}...` : getLocal("Add")}
              </Button>
            </InputGroup>
          </Col>
        </Row>
      </form>
      <div id="showOnLargeScreen">
        <Table striped bordered hover size="sm" className="three-dtbl">
          <thead>
            <tr>
              <th>{getLocal("Business Type")}</th>
              <th>{getLocal("Product Type")}</th>
              <th>{getLocal("Description")}</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {!loading && userProducts.length > 0 ? (
              userProducts.map((ss, index) => (
                <tr key={index}>
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
                      className="btn-sm btn btn-warning "
                      style={{ float: "none", margin: "0" }}
                      onClick={() => RemoveProduct(ss)}
                    >
                      {" "}
                      {getLocal("Remove")}
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={3} className="text-center">
                  <strong>
                    {loading
                      ? `${getLocal("Loading")} ...`
                      : getLocal("No Products to show")}
                  </strong>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      </div>
      <div id="showOnSmallScreen">
        {!loading && userProducts.length ? (
          userProducts.map((ss, i) => (
            <Card key={i} className="card__margin-bottom">
              <Card.Header>
                {businessTypes.find((b) => b.id == ss.businessType)
                  ? businessTypes.find((b) => b.id == ss.businessType).name
                  : ""}
              </Card.Header>
              <Card.Body>
                <Row>
                  <Col>{getLocal("Business Type")}</Col>
                  <Col>
                    {businessTypes.find((b) => b.id == ss.businessType)
                      ? businessTypes.find((b) => b.id == ss.businessType).name
                      : ""}
                  </Col>
                </Row>
                <Row>
                  <Col>{getLocal("Product Type")}</Col>
                  <Col>{ss.productName}</Col>
                </Row>
                <Row>
                  <Col>{getLocal("Description")}</Col>
                  <Col>{ss.description}</Col>
                </Row>
              </Card.Body>
            </Card>
          ))
        ) : (
          <Card className="card__margin-bottom">
            <strong>
              {loading
                ? `${getLocal("Loading")} ...`
                : getLocal("No Products to show")}
            </strong>
          </Card>
        )}
      </div>
    </>
  );
}
