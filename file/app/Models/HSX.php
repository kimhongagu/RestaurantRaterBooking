<?php
namespace App\Models;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;

class HangSanXuat extends Model
{
    protected $table = 'hsx';
    protected $fillable = [
    	'tenhsx',
    	'tenhsx_slug',
    	'sodienthoai',
        'diachi',
    ];
	public function VanPhongPham(): HasMany
	{
		return $this->hasMany(VanPhongPham::class, 'hangsanxuat_id', 'id');
	}
}