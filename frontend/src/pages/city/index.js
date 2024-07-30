import City from "./City";
export default City;

export async function loader(name) {
	try {
			const res = await fetch(`/api/city/${name}`, {
			});
			if (res.status === 200) {
					const city = await res.json();
					const value = {
							city: city,
							status: 200
					}
					console.log(city);
					return value;
			} else if ( res.status === 404 ) {
					return {status: 404}
			} else {
					return {status: 401}
			}
	} catch (e) {
			console.log(e);
	}
}